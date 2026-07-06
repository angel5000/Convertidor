using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ConvertidorImagenes
{
    /// <summary>
    /// Panel con esquinas redondeadas y borde opcional.
    /// NO usa Region para recortar — Region causa rectángulos negros
    /// durante el redimensionamiento porque el área nueva queda sin
    /// pintar hasta que OnPaint ejecuta. En su lugar, pintamos el
    /// fondo del padre en toda el área y el rectángulo redondeado
    /// encima, de modo que cada píxel siempre tiene color.
    /// </summary>
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 12;
        public Color BorderColor { get; set; } = Color.Transparent;
        public int BorderThickness { get; set; } = 1;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.SupportsTransparentBackColor, true);
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            if (d <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 1. Llenar TODA el área con el color real del fondo.
            // Si el padre es transparente, buscamos el primer ancestro con color sólido,
            // de lo contrario Graphics.Clear(Color.Transparent) dibuja negro.
            Color parentBg = SystemColors.Control;
            Control p = Parent;
            while (p != null)
            {
                if (p.BackColor != Color.Transparent)
                {
                    parentBg = p.BackColor;
                    break;
                }
                p = p.Parent;
            }
            e.Graphics.Clear(parentBg);

            // 2. Pintar el rectángulo redondeado con nuestro BackColor.
            //    Lo hacemos aquí (no en OnPaint) para que los controles
            //    hijos transparentes vean el fondo correcto.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = GetRoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius))
            using (var brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Solo dibujamos el borde; el fondo ya se pintó en OnPaintBackground.
            if (BorderColor != Color.Transparent && BorderThickness > 0)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (var path = GetRoundedRect(rect, CornerRadius))
                using (var pen = new Pen(BorderColor, BorderThickness))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            base.OnPaint(e);
        }
    }

    /// <summary>
    /// Botón plano sin bordes con estado hover, usado en los ítems
    /// de navegación de la barra lateral.
    /// Color.Transparent NO funciona en Button de WinForms: produce
    /// rectángulos negros y controles invisibles. Usamos el color
    /// real del fondo del sidebar (blanco) como NormalColor.
    /// </summary>
    public class NavButton : Button
    {
        public Color HoverColor { get; set; } = Color.FromArgb(238, 240, 250);
        public Color NormalColor { get; set; } = Color.White;
        public Color SelectedColor { get; set; } = Color.FromArgb(230, 233, 250);
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                // Cambiar BackColor aquí, NO en OnPaint (evita loop de invalidación).
                BackColor = _selected ? SelectedColor : NormalColor;
            }
        }

        public NavButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.AllPaintingInWmPaint, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = HoverColor;
            FlatAppearance.MouseDownBackColor = HoverColor;
            BackColor = NormalColor;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageAlign = ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            Cursor = Cursors.Hand;
            Padding = new Padding(12, 0, 0, 0);
        }

        // OnPaint ya no asigna BackColor — eso creaba un loop de
        // invalidación que hacía que los botones no se renderizaran
        // en la carga inicial.
    }
}
