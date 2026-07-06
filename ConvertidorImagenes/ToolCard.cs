using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConvertidorImagenes
{
    /// <summary>
    /// Tarjeta de herramienta usada en "Herramientas utilizadas recientemente"
    /// y en "Herramientas favoritas". El estilo cambia según el modo.
    /// </summary>
    public class ToolCard : RoundedPanel
    {
        public event EventHandler CardClick;
        public event EventHandler StarClick;

        private readonly PictureBox iconCircle;
        private readonly PictureBox icon;
        private readonly Label titleLabel;
        private readonly Label subtitleLabel;
        private readonly RoundedPanel progressBar;
        private readonly PictureBox star;

        private Image starFilled;
        private Image starOutline;
        private bool isFavorite;

        /// <summary>
        /// Controla si la estrella se muestra. Se usa para el "modo gestión":
        /// fuera de ese modo la estrella permanece oculta.
        /// </summary>
        public bool StarVisible
        {
            get => star != null && star.Visible;
            set { if (star != null) star.Visible = value; }
        }

        public ToolCard(string title, string subtitle, Image iconImage, Color accentColor,
                         bool favoriteMode, bool isFav, Image starFilledImg, Image starOutlineImg)
        {
            BackColor = Color.White;
            CornerRadius = 14;
            BorderColor = Color.FromArgb(235, 236, 242);
            BorderThickness = 1;
            Cursor = Cursors.Hand;

            starFilled = starFilledImg;
            starOutline = starOutlineImg;
            isFavorite = isFav;

            int circleSize = favoriteMode ? 56 : 60;

            iconCircle = new PictureBox
            {
                Size = new Size(circleSize, circleSize),
                Location = new Point(favoriteMode ? (Width - circleSize) / 2 : 20, 20),
                BackColor = Color.Transparent, // Fondo transparente para evitar esquinas negras
                SizeMode = PictureBoxSizeMode.CenterImage,
                Cursor = Cursors.Hand
            };

            // Dibujar el círculo nosotros mismos en lugar de usar Region
            iconCircle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(ControlPaint.Light(accentColor, 0.9f)))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, iconCircle.Width - 1, iconCircle.Height - 1);
                }
            };

            icon = new PictureBox
            {
                Image = iconImage,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(circleSize - 24, circleSize - 24),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            icon.Location = new Point((iconCircle.Width - icon.Width) / 2, (iconCircle.Height - icon.Height) / 2);
            iconCircle.Controls.Add(icon);

            titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 32, 40),
                AutoSize = false,
                TextAlign = favoriteMode ? ContentAlignment.TopCenter : ContentAlignment.TopLeft,
                Cursor = Cursors.Hand
            };

            subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(150, 154, 165),
                AutoSize = false,
                TextAlign = favoriteMode ? ContentAlignment.TopCenter : ContentAlignment.TopLeft,
                Cursor = Cursors.Hand
            };

            Controls.Add(iconCircle);
            Controls.Add(titleLabel);
            if (!favoriteMode)
            {
                Controls.Add(subtitleLabel);

                progressBar = new RoundedPanel
                {
                    BackColor = accentColor,
                    CornerRadius = 3,
                    Size = new Size(1, 5)
                };
                Controls.Add(progressBar);
            }
            else
            {
                star = new PictureBox
                {
                    Image = starFilled,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(18, 18),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                    Visible = false // solo se muestra en "modo gestión"
                };
                star.Click += (s, e) =>
                {
                    // En modo gestión, la estrella siempre representa
                    // "quitar de favoritos": al hacer clic, se notifica
                    // al contenedor para que elimine esta tarjeta.
                    StarClick?.Invoke(this, EventArgs.Empty);
                };
                Controls.Add(star);
            }

            this.Resize += (s, e) => Layout(favoriteMode);
            Layout(favoriteMode);

            // La estrella tiene su propio comportamiento (alternar favorito) y
            // no debe además disparar la navegación de la tarjeta.
            iconCircle.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            icon.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            titleLabel.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            subtitleLabel.Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
            Click += (s, e) => CardClick?.Invoke(this, EventArgs.Empty);
        }

        private void Layout(bool favoriteMode)
        {
            if (favoriteMode)
            {
                iconCircle.Location = new Point((Width - iconCircle.Width) / 2, 24);
                titleLabel.Size = new Size(Width - 16, 34);
                titleLabel.Location = new Point(8, iconCircle.Bottom + 12);
                if (star != null)
                {
                    star.Location = new Point(Width - star.Width - 12, 12);
                    star.BringToFront();
                }
            }
            else
            {
                titleLabel.Size = new Size(Width - 40, 40);
                titleLabel.Location = new Point(20, iconCircle.Bottom + 12);
                subtitleLabel.Size = new Size(Width - 40, 18);
                subtitleLabel.Location = new Point(20, titleLabel.Bottom + 2);
                if (progressBar != null)
                {
                    progressBar.Size = new Size(Width - 40, 5);
                    progressBar.Location = new Point(20, Height - 24);
                }
            }
        }

        // Se eliminó MakeCircle porque el uso de Region en WinForms causa
        // artefactos negros (las esquinas no se repintan correctamente
        // al no estar en la región del control). Ahora usamos el evento Paint.
    }
}
