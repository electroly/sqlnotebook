// SQL Notebook
// Copyright (C) 2017 Brian Luft
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
            grid.Dock = DockStyle.Fill;
            grid.SheetTabVisible = false;
            grid.Readonly = readOnly;
            foreach (var color in _colors) {
                grid.ControlStyle.SetColor(color.Key, color.Value);
            }
        }
    }
}