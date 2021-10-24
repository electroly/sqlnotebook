﻿using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlNotebook.Import.Xls {
    public sealed class XlsInput {
        public string FilePath;
        public List<XlsWorksheetInfo> Worksheets;

        public static XlsInput Load(string xlsFilePath) {
            if (!File.Exists(xlsFilePath)) {
                throw new FileNotFoundException();
            }

            List<XlsWorksheetInfo> worksheets = new();
            XlsUtil.WithWorkbook(xlsFilePath, workbook => {
                var numSheets = workbook.NumberOfSheets;

                if (numSheets == 0) {
                    throw new Exception("This workbook does not contain any sheets.");
                }

                for (var i = 0; i < numSheets; i++) {
                    worksheets.Add(XlsWorksheetInfo.Load(workbook, i));
                }
            });

            return new() {
                FilePath = xlsFilePath,
                Worksheets = worksheets
            };
        }
    }
}