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
using System.Data;
using System.IO;
using System.Linq;
using Mono.Terminal;
using SqlNotebookCore;

// a quick and dirty command line interface for SqlNotebook
namespace Snb {
    public static class Program {
        public static void Main(string[] args) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            var filePath = Path.GetTempFileName();
            Console.WriteLine("SQL Notebook");
            try {
                using (var notebook = new Notebook(filePath)) {
                    var lineEditor = new LineEditor("sqlnotebook");
                    string input;

                    while ((input = lineEditor.Edit("> ", "")) != null) {
                        if (input == "exit") {
                            break;
                        } else if (input.Trim() == "") {
                            Console.WriteLine();
                            continue;
                        }
                        try {
                            using (var dt = notebook.Query(input)) {
                                if (dt == null) {
                                    Console.WriteLine("No results.");
                                } else if (dt.Columns.Count == 0) {
                                    Console.WriteLine("Done.");
                                } else {
                                    var columnWidths =
                                        from row in dt.Rows.Cast<DataRow>()
                                        from colIndex in Enumerable.Range(0, dt.Columns.Count)
                                        let length = row[colIndex].ToString().Length
                                        group length by colIndex into grp
                                        let valueMax = Math.Max(grp.Max(), dt.Columns[grp.Key].ColumnName.Length)
                                        select new { ColIndex = grp.Key, MaxLength = valueMax };

                                    var paddedHeaders =
                                        from colIndex in Enumerable.Range(0, dt.Columns.Count)
                                        join x in columnWidths on colIndex equals x.ColIndex
                                        select dt.Columns[colIndex].ColumnName.PadRight(x.MaxLength);
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(string.Join("  ", paddedHeaders));
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    foreach (DataRow row in dt.Rows) {
                                        var paddedValues =
                                            from colIndex in Enumerable.Range(0, dt.Columns.Count)
                                            join x in columnWidths on colIndex equals x.ColIndex
                                            select row.ItemArray[colIndex].ToString().PadRight(x.MaxLength);
                                        Console.WriteLine(string.Join("  ", paddedValues));
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine($"({dt.Rows.Count} row{(dt.Rows.Count == 1 ? "" : "s")}.)");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }
                            }
                        } catch (Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine();
                    }
                }
            } finally {
                File.Delete(filePath);
            }
        }
    }
}
