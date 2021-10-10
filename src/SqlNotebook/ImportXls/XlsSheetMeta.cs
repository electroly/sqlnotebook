using System;

namespace SqlNotebook.ImportXls {
    public sealed class XlsSheetMeta {
        public int Index { get; set; }
        public string OriginalName { get; set; }
        public string NewName { get; set; }
        public bool ToBeImported { get; set; }

        public ImportTableExistsOption ImportTableExists { get; set; } = ImportTableExistsOption.DropTable;
        public string ImportTableExistsString {
            get => ImportTableExists.GetDescription();
            set => ImportTableExists = ImportTableExists.GetValueFromDescription(value);
        }

        public ImportConversionFailOption ImportConversionFail { get; set; } = ImportConversionFailOption.Abort;
        public string ImportConversionFailString {
            get => ImportConversionFail.GetDescription();
            set => ImportConversionFail = ImportConversionFail.GetValueFromDescription(value);
        }
    }
}
