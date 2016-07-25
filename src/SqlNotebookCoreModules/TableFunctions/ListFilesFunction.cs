// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlNotebookCoreModules.TableFunctions {
    public sealed class ListFilesFunction : CustomTableFunction {
        public override string Name => "list_files";

        public override string CreateTableSql =>
            @"CREATE TABLE list_files (_root_path HIDDEN, _recursive HIDDEN,
            file_path PRIMARY KEY, folder, filename, extension, modified_date)";

        public override int HiddenColumnCount => 2;

        public override IEnumerable<object[]> Execute(object[] hiddenValues) {
            var rootPathObj = hiddenValues[0];
            var recursiveObj = hiddenValues[1] ?? 0L;
            if (rootPathObj == null) {
                throw new Exception("LIST_FILES: The \"root-path\" argument is required.");
            }
            if (!(rootPathObj is string)) {
                throw new Exception("LIST_FILES: The \"root-path\" argument must be a string.");
            }
            if (!(recursiveObj is Int64)) {
                throw new Exception("LIST_FILES: The \"recursive\" argument must be an integer.");
            }
            var rootPath = (string)rootPathObj;
            var recursive = (Int64)recursiveObj != 0;

            var folderQueue = new Queue<string>();
            var fileQueue = new Queue<string>();

            if (recursive) {
                folderQueue.Enqueue(rootPath);
            } else {
                foreach (var filePath in Directory.GetFiles(rootPath)) {
                    fileQueue.Enqueue(filePath);
                }
            }

            while (fileQueue.Any() || folderQueue.Any()) {
                if (fileQueue.Any()) {
                    yield return GetRow(fileQueue.Dequeue(), rootPath, recursive);
                } else if (folderQueue.Any()) {
                    var folder = folderQueue.Dequeue();
                    try {
                        foreach (var filePath in Directory.GetFiles(folder)) {
                            fileQueue.Enqueue(filePath);
                        }
                        foreach (var subfolder in Directory.GetDirectories(folder)) {
                            folderQueue.Enqueue(subfolder);
                        }
                    } catch {
                        // might get ourselves into trouble with permissions, folders being while we search, etc.
                        // just skip folders when that happens.
                    }
                }
            }
        }

        private static object[] GetRow(string filePath, string rootPath, bool recursive) {
            return new object[] {
                rootPath,
                recursive ? 1L : 0L,
                filePath,
                Path.GetDirectoryName(filePath),
                Path.GetFileName(filePath),
                Path.GetExtension(filePath),
                File.GetLastWriteTime(filePath)
            };
        }
    }
}
