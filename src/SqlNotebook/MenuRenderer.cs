using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook;

public sealed class MenuRenderer : ToolStripSystemRenderer {
    private readonly Font _font = new("Segoe UI", 9);
    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
        var color = e.Item.Enabled ? Color.Black : Color.FromArgb(109, 109, 109);

        var rect = e.TextRectangle;
        var menuItem = e.Item as ToolStripMenuItem;
        if (menuItem != null && e.Text != e.Item.Text) {
            rect.Offset(10, 0); // move the accelerator key text
        }

        TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, rect, color, e.TextFormat);
    }

    private static Pen _menuBorderPen = new Pen(Color.FromArgb(204, 204, 204));
    private static Brush _separatorBrush = new SolidBrush(Color.FromArgb(215, 215, 215));
    private static Brush _disabledSelectedBrush = new SolidBrush(Color.FromArgb(230, 230, 230));
    private static Brush _selectedBrush = new SolidBrush(Color.FromArgb(145, 201, 247));
    private static Brush _topMenuBrush = new SolidBrush(Color.FromArgb(250, 250, 250));
    private static Pen _topMenuPen = new Pen(Color.FromArgb(250, 250, 250));
    private static Brush _menuHoverBgBrush = new SolidBrush(Color.FromArgb(229, 243, 255));
    private static Pen _menuHoverBorderPen = new Pen(Color.FromArgb(204, 232, 255));
    private static Brush _menuOpenBgBrush = new SolidBrush(Color.FromArgb(204, 232, 255));
    private static Pen _menuOpenBorderPen = new Pen(Color.FromArgb(153, 209, 255));
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
        var rect = e.Item.ContentRectangle;

        var isTopLevelMenu = e.Item.GetCurrentParent() is MenuStrip;
        if (isTopLevelMenu) {
            rect.X += 2;
            rect.Width -= 5;
            rect.Y--;
            rect.Height++;
            var menuItem = (ToolStripMenuItem)e.Item;
            Pen borderPen;
            Brush fillBrush;
            if (!menuItem.Enabled) {
                borderPen = _topMenuPen;
                fillBrush = _topMenuBrush;
            } else if (menuItem.DropDown?.Visible ?? false) {
                borderPen = _menuOpenBorderPen;
                fillBrush = _menuOpenBgBrush;
            } else if (e.Item.Selected) {
                borderPen = _menuHoverBorderPen;
                fillBrush = _menuHoverBgBrush;
            } else {
                borderPen = _topMenuPen;
                fillBrush = _topMenuBrush;
            }
            e.Graphics.FillRectangle(fillBrush, rect);
            e.Graphics.DrawRectangle(borderPen, rect);
            return;
        } else {
            rect.Y--;
            rect.Height += 2;
            rect.X++;
            rect.Width--;
            Brush brush;
            if (e.Item.Selected) {
                brush = e.Item.Enabled ? _selectedBrush : _disabledSelectedBrush;
            } else {
                brush = SystemBrushes.Control;
            }
            e.Graphics.FillRectangle(brush, rect);
        }
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
        var rect = e.Item.ContentRectangle;
        rect.Height = 1;
        rect.Y++;
        rect.X += 28;
        rect.Width -= 27;
        e.Graphics.FillRectangle(_separatorBrush, rect);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
        var dd = e.ToolStrip as ToolStripDropDown;
        if (dd != null) {
            var rect = new Rectangle(0, 0, dd.Size.Width - 1, dd.Size.Height - 1);
            e.Graphics.DrawRectangle(_menuBorderPen, rect);
        }
    }
        
    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) {
        var rect = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 2);
        if (e.Item.Pressed) {
            e.Graphics.FillRectangle(_menuOpenBgBrush, rect);
            e.Graphics.DrawRectangle(_menuOpenBorderPen, rect);
        } else if (e.Item.Selected) {
            e.Graphics.FillRectangle(_menuHoverBgBrush, rect);
            e.Graphics.DrawRectangle(_menuHoverBorderPen, rect);
        } else {
            e.Graphics.FillRectangle(_topMenuBrush, rect);
        }

        DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rect, Color.Black, ArrowDirection.Down));
    }
}
