using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SqlNotebookScript.Utils;

public static class NotebookTempFiles {
    private static readonly Lazy<(string FilenamePrefix, string Path)> _dir = new(() => {
        using var p = Process.GetCurrentProcess();
        var filenamePrefix = $"{p.Id}.";
        var path = Path.Combine(Path.GetTempPath(), "SqlNotebookTemp");
        Directory.CreateDirectory(path);
        DeleteFilesFromThisSession(filenamePrefix, path);
        DeleteFilesFromPreviousSessions(path);
        return (filenamePrefix, path);
    });

    public static string GetTempFilePath(string extension) {
        var dir = _dir.Value;
        var filePath = Path.Combine(dir.Path, $"{dir.FilenamePrefix}{Guid.NewGuid()}{extension}");
        File.WriteAllBytes(filePath, Array.Empty<byte>());
        return filePath;
    }

    public static void DeleteFilesFromThisSession() {
        var dir = _dir.Value;
        DeleteFilesFromThisSession(dir.FilenamePrefix, dir.Path);
    }

    private static void DeleteFilesFromThisSession(string filenamePrefix, string path) {
        try {
            foreach (var filePath in Directory.GetFiles(path)) {
                var filename = Path.GetFileName(filePath);
                if (filename.StartsWith(filenamePrefix)) {
                    try {
                        File.Delete(filePath);
                    } catch { }
                }
            }
        } catch { }
    }

    private static void DeleteFilesFromPreviousSessions(string path) {
        // This is annoying in the debugger due to the exception that gets thrown below, so skip this when a debugger
        // is attached.
        if (Debugger.IsAttached) {
            return;
        }

        // Track process IDs we've seen before so we don't call GetProcessById more than once per process.
        HashSet<int> pids = new();

        try {
            foreach (var filePath in Directory.GetFiles(path)) {
                var filename = Path.GetFileName(filePath);

                // If the first part of the filename matches the PID of a running process, then skip the file for now.
                var firstPart = filename.Split('.')[0];
                if (int.TryParse(firstPart, out var pid)) {
                    if (pids.Contains(pid)) {
                        // We know this PID exists. Don't delete the file.
                        continue;
                    }

                    try {
                        Process.GetProcessById(pid);
                        // If we made it this far, then a process with this PID actually exists. Don't delete the file.
                        pids.Add(pid);
                        continue;
                    } catch { }
                }

                try {
                    File.Delete(filePath);
                } catch { }
            }
        }
        catch { }
    }
}
