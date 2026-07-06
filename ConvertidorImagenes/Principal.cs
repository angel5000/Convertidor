using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ConvertidorImagenes.Controllers;

namespace ConvertidorImagenes
{
    public partial class Principal : Form
    {
        private readonly NavigationController navigationController = new NavigationController();

        // Guardamos referencia a los nav buttons para el resaltado y el filtro de búsqueda.
        private List<NavButton> navButtons;
        private List<ToolCard> recentCards;
        private List<ToolCard> favoriteCards;
        private bool modoGestion = false;

        // Bandera para evitar re-layout durante el arrastre de redimensionamiento.
        private bool isResizing = false;

        public Principal()
        {
            // Habilitar double-buffering antes de crear controles.
            // NO incluir UserPaint: ese flag indica que el Form pinta
            // todo manualmente, lo cual interfiere con los controles hijos.
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                          | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            // Suspender el layout durante la construcción masiva de controles.
            this.SuspendLayout();
            panelContent.SuspendLayout();
            panelSidebar.SuspendLayout();

            CargarIconos();
            ConstruirNavegacion();
            ConstruirTarjetasRecientes();
            ConstruirTarjetasFavoritas();
            ConfigurarPlaceholderBuscar();

            panelSidebar.ResumeLayout(true);
            panelContent.ResumeLayout(true);
            this.ResumeLayout(true);

            // Habilitar double-buffering recursivo en todos los hijos.
            EnableDoubleBuffering(this);

            // Forzar repintado completo para que el sidebar se dibuje
            // sin necesidad de redimensionar la ventana.
            this.Refresh();

            // Suscribirse a ResizeEnd para reconstruir tarjetas
            // solo al terminar de arrastrar el borde.
            this.ResizeEnd += Principal_ResizeEnd;
        }

        /// <summary>
        /// Habilita DoubleBuffered en un control y todos sus hijos recursivamente.
        /// Usa reflexión porque Panel.DoubleBuffered es protected.
        /// </summary>
        private static void EnableDoubleBuffering(Control control)
        {
            var prop = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
                prop.SetValue(control, true, null);

            foreach (Control child in control.Controls)
                EnableDoubleBuffering(child);
        }

        private void CargarIconos()
        {
            pictureLogo.Image = IconLoader.Load("logo");
            pictureHome.Image = IconLoader.Load("home");
            pictureSearch.Image = IconLoader.Load("search");
            pictureBell.Image = IconLoader.Load("bell");
            pictureClock.Image = IconLoader.Load("clock");
            pictureBulb.Image = IconLoader.Load("bulb");
            btnCerrarTip.Image = IconLoader.Load("close");
        }

        private const string PlaceholderBuscar = "Buscar herramienta...";

        private void ConfigurarPlaceholderBuscar()
        {
            // TextBox.PlaceholderText no existe en WinForms de .NET Framework
            // (solo en .NET 5+), así que lo simulamos con texto gris + Enter/Leave.
            txtBuscar.Text = PlaceholderBuscar;
            txtBuscar.ForeColor = Color.FromArgb(160, 163, 172);
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == PlaceholderBuscar)
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = Color.FromArgb(60, 62, 70);
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = PlaceholderBuscar;
                txtBuscar.ForeColor = Color.FromArgb(160, 163, 172);
            }
        }

        private void ConstruirNavegacion()
        {
            navButtons = new List<NavButton> { navConvertidor, navOffice, navOcr, navResize, navPlantillas, navAyuda };

            navConvertidor.Image = IconLoader.Load("logo");
            navOffice.Image = IconLoader.Load("office");
            navOcr.Image = IconLoader.Load("ocr");
            navResize.Image = IconLoader.Load("resize");
            navPlantillas.Image = IconLoader.Load("bulb"); // Usamos el bombillo para "Smart Templates"
            navAyuda.Image = IconLoader.Load("help");

            foreach (var nav in navButtons)
            {
                if (nav.Image != null)
                {
                    nav.ImageList = null;
                    nav.Image = new Bitmap(nav.Image, new Size(20, 20));
                }
            }
        }

        private void ConstruirTarjetasRecientes()
        {
            panelRecentCards.SuspendLayout();
            panelRecentCards.Controls.Clear();
            recentCards = new List<ToolCard>();

            var datos = new (string titulo, string subtitulo, string icono, Color color, EventHandler accion)[]
            {
                ("Convertidor de\nImágenes", "Hace 5 min", "logo", Color.FromArgb(79, 124, 255),
                    (s, e) => navigationController.Show<Form1>(this)),
                ("Office a PDF /\nPDF a Office", "Hace 30 min", "office", Color.FromArgb(139, 92, 246),
                    (s, e) => navigationController.Show<Offices>(this)),
                ("Reconocimiento Óptico\nde Caracteres (OCR)", "Hace 1 hora", "ocr", Color.FromArgb(20, 184, 166),
                    (s, e) => navigationController.Show<OCR>(this)),
                ("Redimensiona y\ncorta Imágenes", "Hace 2 horas", "resize", Color.FromArgb(245, 158, 11),
                    (s, e) => navigationController.Show<redimension>(this)),
            };

            int count = datos.Length;
            int gap = 20;
            int cardWidth = (panelRecentCards.Width - gap * (count - 1)) / count;

            for (int i = 0; i < count; i++)
            {
                var d = datos[i];
                var card = new ToolCard(d.titulo, d.subtitulo, IconLoader.Load(d.icono), d.color,
                                         favoriteMode: false, isFav: false, starFilledImg: null, starOutlineImg: null)
                {
                    Location = new Point(i * (cardWidth + gap), 0),
                    Size = new Size(cardWidth, panelRecentCards.Height)
                };
                card.CardClick += d.accion;
                panelRecentCards.Controls.Add(card);
                recentCards.Add(card);
            }
            panelRecentCards.ResumeLayout(true);
        }

        private void ConstruirTarjetasFavoritas()
        {
            panelFavCards.SuspendLayout();
            panelFavCards.Controls.Clear();
            favoriteCards = new List<ToolCard>();

            Image starFilled = IconLoader.Load("star_filled");
            Image starOutline = IconLoader.Load("star_outline");

            var datos = new (string titulo, string icono, Color color, bool favorito, EventHandler accion)[]
            {
                ("Convertidor de\nImágenes", "logo", Color.FromArgb(79, 124, 255), true,
                    (s, e) => navigationController.Show<Form1>(this)),
                ("Office a PDF / PDF\na Office (Experimental)", "office", Color.FromArgb(139, 92, 246), true,
                    (s, e) => navigationController.Show<Offices>(this)),
                ("Reconocimiento Óptico\nde Caracteres (OCR)", "ocr", Color.FromArgb(20, 184, 166), true,
                    (s, e) => navigationController.Show<OCR>(this)),
                ("Redimensiona y\ncorta Imágenes", "resize", Color.FromArgb(245, 158, 11), true,
                    (s, e) => navigationController.Show<redimension>(this)),
                ("Ayuda", "help", Color.FromArgb(59, 130, 246), false,
                    (s, e) => MessageBox.Show("Pronto 7u7.")),
            };

            int count = datos.Length;
            int gap = 20;
            int cardWidth = (panelFavCards.Width - gap * (count - 1)) / count;

            for (int i = 0; i < count; i++)
            {
                var d = datos[i];
                var card = new ToolCard(d.titulo, "", IconLoader.Load(d.icono), d.color,
                                         favoriteMode: true, isFav: d.favorito,
                                         starFilledImg: starFilled, starOutlineImg: starOutline)
                {
                    Location = new Point(i * (cardWidth + gap), 0),
                    Size = new Size(cardWidth, panelFavCards.Height)
                };
                card.CardClick += d.accion;
                card.StarClick += (s, e) => QuitarDeFavoritos((ToolCard)s);
                card.StarVisible = modoGestion;
                panelFavCards.Controls.Add(card);
                favoriteCards.Add(card);
            }
            panelFavCards.ResumeLayout(true);
        }

        /// <summary>
        /// Quita una tarjeta de la sección "Herramientas favoritas" y
        /// reacomoda (reflow) las tarjetas restantes para llenar el espacio.
        /// </summary>
        private void QuitarDeFavoritos(ToolCard card)
        {
            favoriteCards.Remove(card);
            panelFavCards.Controls.Remove(card);
            card.Dispose();
            ReflowFavoritas();
        }

        private void ReflowFavoritas()
        {
            int count = favoriteCards.Count;
            if (count == 0) return;

            int gap = 20;
            int cardWidth = (panelFavCards.Width - gap * (count - 1)) / count;

            for (int i = 0; i < count; i++)
            {
                favoriteCards[i].Location = new Point(i * (cardWidth + gap), 0);
                favoriteCards[i].Size = new Size(cardWidth, panelFavCards.Height);
            }
        }

        private void ReflowRecientes()
        {
            if (recentCards == null) return;
            int count = recentCards.Count;
            if (count == 0) return;

            int gap = 20;
            int cardWidth = (panelRecentCards.Width - gap * (count - 1)) / count;

            for (int i = 0; i < count; i++)
            {
                recentCards[i].Location = new Point(i * (cardWidth + gap), 0);
                recentCards[i].Size = new Size(cardWidth, panelRecentCards.Height);
            }
        }

        // ---- Optimización de redimensionamiento ----

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Marcar que estamos en proceso de resize.
            // El re-layout costoso se hace al soltar (ResizeEnd).
            if (!isResizing && WindowState == FormWindowState.Normal)
            {
                isResizing = true;
            }
        }

        private void Principal_ResizeEnd(object sender, EventArgs e)
        {
            isResizing = false;
            // Recalcular posición/tamaño de las tarjetas tras el resize.
            panelRecentCards.SuspendLayout();
            panelFavCards.SuspendLayout();

            ReflowRecientes();
            ReflowFavoritas();

            panelFavCards.ResumeLayout(true);
            panelRecentCards.ResumeLayout(true);
        }

        // ---------------- Navegación lateral ----------------

        private void btnInicio_Click(object sender, EventArgs e)
        {
            SeleccionarNav(null);
        }

        private void navConvertidor_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navConvertidor);
            navigationController.Show<Form1>(this);
        }

        private void navOffice_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navOffice);
            navigationController.Show<Offices>(this);
        }

        private void navOcr_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navOcr);
            navigationController.Show<OCR>(this);
        }

        private void navResize_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navResize);
            navigationController.Show<redimension>(this);
        }

        private void navPlantillas_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navPlantillas);
            navigationController.Show<Plantillas>(this);
        }

        private void navAyuda_Click(object sender, EventArgs e)
        {
            SeleccionarNav(navAyuda);
            MessageBox.Show("Pronto 7u7.");
        }

        private void SeleccionarNav(NavButton activo)
        {
            if (navButtons == null) return;
            foreach (var nav in navButtons)
            {
                nav.Selected = (nav == activo);
            }
        }

        // ---------------- Cabecera ----------------

        private void btnBell_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No tienes notificaciones nuevas.");
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            modoGestion = !modoGestion;

            if (favoriteCards != null)
            {
                foreach (var card in favoriteCards)
                {
                    card.StarVisible = modoGestion;
                }
            }

            labelGestionar.Text = modoGestion ? "✓  Listo" : "✎  Gestionar";
            btnGestionar.BackColor = modoGestion
                ? Color.FromArgb(109, 91, 208)
                : Color.FromArgb(238, 233, 253);
            labelGestionar.ForeColor = modoGestion
                ? Color.White
                : Color.FromArgb(109, 91, 208);
        }

        private void btnCerrarTip_Click(object sender, EventArgs e)
        {
            panelTip.Visible = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string textoActual = txtBuscar.Text == PlaceholderBuscar ? "" : txtBuscar.Text;
            string texto = textoActual.Trim().ToLower();

            if (navButtons != null)
            {
                foreach (var nav in navButtons)
                {
                    nav.Visible = string.IsNullOrEmpty(texto) ||
                                  nav.Text.ToLower().Contains(texto);
                }
            }
        }

        // ---------------- Varios ----------------

        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            navigationController.CloseApplication();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Convertidor de Imágenes\nVersión Alpha.");
        }
    }
}

