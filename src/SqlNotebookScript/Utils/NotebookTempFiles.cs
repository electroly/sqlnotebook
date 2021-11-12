using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SqlNotebookScript.Utils;

public static class NotebookTempFiles {
    private static readonly object _lock = new();
    
    private static string _filenamePrefix;
    private static string _path;
    private static int _count;

    public static void Init() {
        using var p = Process.GetCurrentProcess();
        _filenamePrefix = $"{p.Id}.";
        _path = Path.Combine(Path.GetTempPath(), "SqlNotebookTemp");
        Directory.CreateDirectory(_path);
        DeleteFilesFromThisSession();
        DeleteFilesFromPreviousSessions();
    }

    public static string GetTempFilePath(string extension) {
        lock (_lock) {
            var filePath = Path.Combine(_path, $"{_filenamePrefix}{++_count}{extension}");
            File.WriteAllBytes(filePath, Array.Empty<byte>());
            return filePath;
        }
    }

    public static void DeleteFilesFromThisSession() {
        try {
            foreach (var filePath in Directory.GetFiles(_path)) {
                var filename = Path.GetFileName(filePath);
                if (filename.StartsWith(_filenamePrefix)) {
                    try {
                        File.Delete(filePath);
                    } catch { }
                }
            }
        } catch { }
    }

    private static void DeleteFilesFromPreviousSessions() {
        // This is annoying in the debugger due to the exception that gets thrown below, so skip this when a debugger
        // is attached.
        if (Debugger.IsAttached) {
            return;
        }

        // Track process IDs we've seen before so we don't call GetProcessById more than once per process.
        HashSet<int> pids = new();

        try {
            foreach (var filePath in Directory.GetFiles(_path)) {
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
