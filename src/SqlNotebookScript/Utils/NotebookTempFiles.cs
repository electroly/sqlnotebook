using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SqlNotebookScript.Utils;

public static class NotebookTempFiles {
    private static readonly Lazy<string> _dir = new(() => {
        using var p = Process.GetCurrentProcess();
        var parent = Path.Combine(Path.GetTempPath(), "SqlNotebookTemp");
        DeleteFilesFromPreviousSessions(parent);
        var path = Path.Combine(parent, $"{p.Id}");
        Directory.CreateDirectory(path);
        return path;
    });

    public static string GetTempDir() => _dir.Value;

    public static string GetTempFilePath(string extension) {
        var dir = _dir.Value;
        var filePath = Path.Combine(dir, $"{Guid.NewGuid()}{extension}");
        File.WriteAllBytes(filePath, Array.Empty<byte>());
        return filePath;
    }

    public static void DeleteFilesFromThisSession() {
        var dir = _dir.Value;
        try {
            Directory.Delete(dir, true);
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
            foreach (var file in Directory.GetFiles(path)) {
                try {
                    File.Delete(file);
                } catch { }
            }

            foreach (var dir in Directory.GetDirectories(path)) {
                // If the dir matches the PID of a running process, then skip it for now.
                if (int.TryParse(Path.GetFileName(dir), out var pid)) {
                    if (pids.Contains(pid)) {
                        // We know this PID exists. Don't delete the file.
                        continue;
                    }

                    try {
                        using (Process.GetProcessById(pid)) {
                            // If we made it this far, then a process with this PID actually exists. Don't delete the file.
                            pids.Add(pid);
                            continue;
                        }
                    } catch { }
                }

                try {
                    Directory.Delete(dir, true);
                } catch { }
            }
        }
        catch { }
    }
}
