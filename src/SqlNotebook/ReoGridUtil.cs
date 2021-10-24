using System.Collections.Generic;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.Graphics;

namespace SqlNotebook {
    public static class ReoGridUtil {
        private static readonly IReadOnlyDictionary<ControlAppearanceColors, SolidColor> _colors =
            new Dictionary<ControlAppearanceColors, SolidColor> {
                [ControlAppearanceColors.ColHeadSplitter] = new SolidColor(0xE5, 0xE5, 0xE5),
                [ControlAppearanceColors.ColHeadNormalStart] = new SolidColor(0xFF, 0xFF, 0xFF),
                [ControlAppearanceColors.ColHeadNormalEnd] = new SolidColor(0xFF, 0xFF, 0xFF),
                [ControlAppearanceColors.ColHeadHoverStart] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadHoverEnd] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadSelectedStart] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadSelectedEnd] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadFullSelectedStart] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadFullSelectedEnd] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadInvalidStart] = new SolidColor(0xFF, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadInvalidEnd] = new SolidColor(0xFF, 0xE0, 0xE0),
                [ControlAppearanceColors.ColHeadText] = new SolidColor(0x00, 0x00, 0x00),
                [ControlAppearanceColors.RowHeadSplitter] = new SolidColor(0xE5, 0xE5, 0xE5),
                [ControlAppearanceColors.RowHeadNormal] = new SolidColor(0xFF, 0xFF, 0xFF),
                [ControlAppearanceColors.RowHeadHover] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.RowHeadSelected] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.RowHeadFullSelected] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.RowHeadInvalid] = new SolidColor(0xFF, 0xE0, 0xE0),
                [ControlAppearanceColors.RowHeadText] = new SolidColor(0x00, 0x00, 0x00),
                [ControlAppearanceColors.LeadHeadNormal] = new SolidColor(0xFF, 0xFF, 0xFF),
                [ControlAppearanceColors.LeadHeadHover] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.LeadHeadSelected] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.LeadHeadIndicatorStart] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.LeadHeadIndicatorEnd] = new SolidColor(0xE0, 0xE0, 0xE0),
                [ControlAppearanceColors.GridLine] = new SolidColor(0xE5, 0xE5, 0xE5)
            };

        public static void InitGrid(ReoGridControl grid, bool readOnly) {
            grid.Cursor = Cursors.Default;
            grid.CellsSelectionCursor = Cursors.Default;
            grid.EntireSheetSelectionCursor = Cursors.Default;
            grid.FullColumnSelectionCursor = Cursors.Default;
            grid.FullRowSelectionCursor = Cursors.Default;
            grid.Dock = DockStyle.Fill;
            grid.SheetTabVisible = false;
            grid.Readonly = readOnly;
            foreach (var color in _colors) {
                grid.ControlStyle.SetColor(color.Key, color.Value);
            }
        }
    }
}
