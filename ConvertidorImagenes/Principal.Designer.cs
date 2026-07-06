namespace ConvertidorImagenes
{
    partial class Principal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelSidebarDivider = new System.Windows.Forms.Panel();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.labelAppTitle = new System.Windows.Forms.Label();
            this.btnInicio = new ConvertidorImagenes.RoundedPanel();
            this.pictureHome = new System.Windows.Forms.PictureBox();
            this.labelInicio = new System.Windows.Forms.Label();
            this.labelHerramientas = new System.Windows.Forms.Label();
            this.navConvertidor = new ConvertidorImagenes.NavButton();
            this.navOffice = new ConvertidorImagenes.NavButton();
            this.navOcr = new ConvertidorImagenes.NavButton();
            this.navResize = new ConvertidorImagenes.NavButton();
            this.navPlantillas = new ConvertidorImagenes.NavButton();
            this.navAyuda = new ConvertidorImagenes.NavButton();
            this.dividerNav = new System.Windows.Forms.Panel();
            this.labelFavoritosNav = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();

            this.panelContent = new System.Windows.Forms.Panel();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.panelSearch = new ConvertidorImagenes.RoundedPanel();
            this.pictureSearch = new System.Windows.Forms.PictureBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBell = new ConvertidorImagenes.RoundedPanel();
            this.pictureBell = new System.Windows.Forms.PictureBox();

            this.panelRecent = new ConvertidorImagenes.RoundedPanel();
            this.pictureClock = new System.Windows.Forms.PictureBox();
            this.labelRecentHeader = new System.Windows.Forms.Label();
            this.panelRecentCards = new System.Windows.Forms.Panel();

            this.labelFavHeader = new System.Windows.Forms.Label();
            this.btnGestionar = new ConvertidorImagenes.RoundedPanel();
            this.labelGestionar = new System.Windows.Forms.Label();
            this.panelFavoritos = new ConvertidorImagenes.RoundedPanel();
            this.panelFavCards = new System.Windows.Forms.Panel();

            this.panelTip = new ConvertidorImagenes.RoundedPanel();
            this.pictureBulb = new System.Windows.Forms.PictureBox();
            this.labelTip = new System.Windows.Forms.Label();
            this.btnCerrarTip = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureClock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBulb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrarTip)).BeginInit();
            this.SuspendLayout();

            // =========================================================
            // SIDEBAR
            // =========================================================
            this.panelSidebar.BackColor = System.Drawing.Color.White;
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(300, 900);
            this.panelSidebar.TabIndex = 0;

            this.panelSidebarDivider.BackColor = System.Drawing.Color.FromArgb(232, 233, 240);
            this.panelSidebarDivider.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebarDivider.Location = new System.Drawing.Point(299, 0);
            this.panelSidebarDivider.Name = "panelSidebarDivider";
            this.panelSidebarDivider.Size = new System.Drawing.Size(1, 900);

            this.pictureLogo.Location = new System.Drawing.Point(24, 24);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(48, 48);
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureLogo.TabStop = false;

            this.labelAppTitle.AutoSize = false;
            this.labelAppTitle.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.labelAppTitle.ForeColor = System.Drawing.Color.FromArgb(30, 32, 40);
            this.labelAppTitle.Location = new System.Drawing.Point(82, 22);
            this.labelAppTitle.Name = "labelAppTitle";
            this.labelAppTitle.Size = new System.Drawing.Size(194, 52);
            this.labelAppTitle.Text = "Convertidor\r\nde Imágenes";

            this.btnInicio.BackColor = System.Drawing.Color.FromArgb(109, 91, 208);
            this.btnInicio.CornerRadius = 10;
            this.btnInicio.Location = new System.Drawing.Point(24, 96);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(252, 48);
            this.btnInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);

            this.pictureHome.Location = new System.Drawing.Point(20, 14);
            this.pictureHome.Name = "pictureHome";
            this.pictureHome.Size = new System.Drawing.Size(20, 20);
            this.pictureHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureHome.BackColor = System.Drawing.Color.Transparent;
            this.pictureHome.TabStop = false;
            this.pictureHome.Click += new System.EventHandler(this.btnInicio_Click);

            this.labelInicio.AutoSize = false;
            this.labelInicio.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelInicio.ForeColor = System.Drawing.Color.White;
            this.labelInicio.Location = new System.Drawing.Point(52, 13);
            this.labelInicio.Size = new System.Drawing.Size(150, 22);
            this.labelInicio.Text = "Inicio";
            this.labelInicio.BackColor = System.Drawing.Color.Transparent;
            this.labelInicio.Click += new System.EventHandler(this.btnInicio_Click);

            this.btnInicio.Controls.Add(this.pictureHome);
            this.btnInicio.Controls.Add(this.labelInicio);

            this.labelHerramientas.AutoSize = false;
            this.labelHerramientas.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelHerramientas.ForeColor = System.Drawing.Color.FromArgb(150, 154, 165);
            this.labelHerramientas.Location = new System.Drawing.Point(24, 160);
            this.labelHerramientas.Size = new System.Drawing.Size(220, 18);
            this.labelHerramientas.Text = "HERRAMIENTAS";

            // Nav buttons
            this.navConvertidor.Location = new System.Drawing.Point(24, 188);
            this.navConvertidor.Size = new System.Drawing.Size(252, 48);
            this.navConvertidor.Text = "  Convertidor de Imágenes";
            this.navConvertidor.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navConvertidor.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navConvertidor.Click += new System.EventHandler(this.navConvertidor_Click);

            this.navOffice.Location = new System.Drawing.Point(24, 244);
            this.navOffice.Size = new System.Drawing.Size(252, 48);
            this.navOffice.Text = "  Office a PDF / PDF a Office";
            this.navOffice.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navOffice.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navOffice.Click += new System.EventHandler(this.navOffice_Click);

            this.navOcr.Location = new System.Drawing.Point(24, 300);
            this.navOcr.Size = new System.Drawing.Size(252, 48);
            this.navOcr.Text = "  Reconocimiento Óptico de Caracteres (OCR)";
            this.navOcr.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navOcr.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navOcr.Click += new System.EventHandler(this.navOcr_Click);

            this.navResize.Location = new System.Drawing.Point(24, 356);
            this.navResize.Size = new System.Drawing.Size(252, 48);
            this.navResize.Text = "  Redimensiona y corta Imágenes";
            this.navResize.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navResize.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navResize.Click += new System.EventHandler(this.navResize_Click);

            // 
            // navPlantillas
            // 
            this.navPlantillas.Location = new System.Drawing.Point(24, 412);
            this.navPlantillas.Size = new System.Drawing.Size(252, 48);
            this.navPlantillas.Text = "  Plantillas Inteligentes";
            this.navPlantillas.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navPlantillas.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navPlantillas.Click += new System.EventHandler(this.navPlantillas_Click);

            this.navAyuda.Location = new System.Drawing.Point(24, 468);
            this.navAyuda.Size = new System.Drawing.Size(252, 48);
            this.navAyuda.Text = "  Ayuda";
            this.navAyuda.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.navAyuda.ForeColor = System.Drawing.Color.FromArgb(40, 42, 50);
            this.navAyuda.Click += new System.EventHandler(this.navAyuda_Click);

            this.dividerNav.BackColor = System.Drawing.Color.FromArgb(235, 236, 242);
            this.dividerNav.Location = new System.Drawing.Point(24, 472);
            this.dividerNav.Size = new System.Drawing.Size(252, 1);

            this.labelFavoritosNav.AutoSize = false;
            this.labelFavoritosNav.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelFavoritosNav.ForeColor = System.Drawing.Color.FromArgb(90, 93, 102);
            this.labelFavoritosNav.Location = new System.Drawing.Point(24, 490);
            this.labelFavoritosNav.Size = new System.Drawing.Size(200, 24);
            this.labelFavoritosNav.Text = "☆  Favoritos";

            this.labelVersion.AutoSize = false;
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelVersion.ForeColor = System.Drawing.Color.FromArgb(120, 124, 135);
            this.labelVersion.Location = new System.Drawing.Point(24, 856);
            this.labelVersion.Size = new System.Drawing.Size(220, 24);
            this.labelVersion.Text = "ⓘ  Versión: Alpha";
            this.labelVersion.Click += new System.EventHandler(this.label1_Click);

            this.panelSidebar.Controls.Add(this.pictureLogo);
            this.panelSidebar.Controls.Add(this.labelAppTitle);
            this.panelSidebar.Controls.Add(this.btnInicio);
            this.panelSidebar.Controls.Add(this.labelHerramientas);
            this.panelSidebar.Controls.Add(this.navConvertidor);
            this.panelSidebar.Controls.Add(this.navOffice);
            this.panelSidebar.Controls.Add(this.navOcr);
            this.panelSidebar.Controls.Add(this.navResize);
            this.panelSidebar.Controls.Add(this.navPlantillas);
            this.panelSidebar.Controls.Add(this.navAyuda);
            this.panelSidebar.Controls.Add(this.dividerNav);
            this.panelSidebar.Controls.Add(this.labelFavoritosNav);
            this.panelSidebar.Controls.Add(this.labelVersion);
            this.panelSidebar.Controls.Add(this.panelSidebarDivider);

            // =========================================================
            // CONTENIDO
            // =========================================================
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(300, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1150, 900);
            this.panelContent.AutoScroll = true;

            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelWelcome.ForeColor = System.Drawing.Color.FromArgb(30, 32, 40);
            this.labelWelcome.Location = new System.Drawing.Point(32, 24);
            this.labelWelcome.Text = "¡Bienvenido! 👋";

            this.labelSubtitle.AutoSize = true;
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 124, 135);
            this.labelSubtitle.Location = new System.Drawing.Point(32, 60);
            this.labelSubtitle.Text = "Selecciona una herramienta para comenzar";

            this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(248, 248, 251);
            this.panelSearch.BorderColor = System.Drawing.Color.FromArgb(230, 231, 238);
            this.panelSearch.BorderThickness = 1;
            this.panelSearch.CornerRadius = 10;
            this.panelSearch.Location = new System.Drawing.Point(690, 24);
            this.panelSearch.Size = new System.Drawing.Size(300, 44);

            this.pictureSearch.Location = new System.Drawing.Point(14, 12);
            this.pictureSearch.Size = new System.Drawing.Size(18, 18);
            this.pictureSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSearch.BackColor = System.Drawing.Color.Transparent;
            this.pictureSearch.TabStop = false;

            this.txtBuscar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtBuscar.ForeColor = System.Drawing.Color.FromArgb(60, 62, 70);
            this.txtBuscar.Location = new System.Drawing.Point(42, 13);
            this.txtBuscar.Size = new System.Drawing.Size(246, 20);
            this.txtBuscar.BackColor = System.Drawing.Color.FromArgb(248, 248, 251);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            this.txtBuscar.Enter += new System.EventHandler(this.txtBuscar_Enter);
            this.txtBuscar.Leave += new System.EventHandler(this.txtBuscar_Leave);

            this.panelSearch.Controls.Add(this.pictureSearch);
            this.panelSearch.Controls.Add(this.txtBuscar);

            this.btnBell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBell.BackColor = System.Drawing.Color.FromArgb(248, 248, 251);
            this.btnBell.BorderColor = System.Drawing.Color.FromArgb(230, 231, 238);
            this.btnBell.BorderThickness = 1;
            this.btnBell.CornerRadius = 10;
            this.btnBell.Location = new System.Drawing.Point(1010, 24);
            this.btnBell.Size = new System.Drawing.Size(44, 44);
            this.btnBell.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBell.Click += new System.EventHandler(this.btnBell_Click);

            this.pictureBell.Location = new System.Drawing.Point(13, 12);
            this.pictureBell.Size = new System.Drawing.Size(20, 20);
            this.pictureBell.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBell.BackColor = System.Drawing.Color.Transparent;
            this.pictureBell.TabStop = false;
            this.pictureBell.Click += new System.EventHandler(this.btnBell_Click);
            this.btnBell.Controls.Add(this.pictureBell);

            // --- Recientes ---
            this.panelRecent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRecent.BackColor = System.Drawing.Color.White;
            this.panelRecent.BorderColor = System.Drawing.Color.FromArgb(235, 236, 242);
            this.panelRecent.BorderThickness = 1;
            this.panelRecent.CornerRadius = 16;
            this.panelRecent.Location = new System.Drawing.Point(32, 96);
            this.panelRecent.Size = new System.Drawing.Size(1086, 292);

            this.pictureClock.Location = new System.Drawing.Point(24, 22);
            this.pictureClock.Size = new System.Drawing.Size(18, 18);
            this.pictureClock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureClock.BackColor = System.Drawing.Color.Transparent;
            this.pictureClock.TabStop = false;

            this.labelRecentHeader.AutoSize = true;
            this.labelRecentHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelRecentHeader.ForeColor = System.Drawing.Color.FromArgb(30, 32, 40);
            this.labelRecentHeader.Location = new System.Drawing.Point(50, 20);
            this.labelRecentHeader.Text = "Herramientas utilizadas recientemente";

            this.panelRecentCards.Location = new System.Drawing.Point(24, 60);
            this.panelRecentCards.Size = new System.Drawing.Size(1038, 210);
            this.panelRecentCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRecentCards.BackColor = System.Drawing.Color.Transparent;

            this.panelRecent.Controls.Add(this.pictureClock);
            this.panelRecent.Controls.Add(this.labelRecentHeader);
            this.panelRecent.Controls.Add(this.panelRecentCards);

            // --- Favoritos header ---
            this.labelFavHeader.AutoSize = true;
            this.labelFavHeader.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.labelFavHeader.ForeColor = System.Drawing.Color.FromArgb(30, 32, 40);
            this.labelFavHeader.Location = new System.Drawing.Point(32, 408);
            this.labelFavHeader.Text = "★  Herramientas favoritas";

            this.btnGestionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGestionar.BackColor = System.Drawing.Color.FromArgb(238, 233, 253);
            this.btnGestionar.CornerRadius = 10;
            this.btnGestionar.Location = new System.Drawing.Point(1030, 402);
            this.btnGestionar.Size = new System.Drawing.Size(120, 36);
            this.btnGestionar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGestionar.Click += new System.EventHandler(this.btnGestionar_Click);

            this.labelGestionar.AutoSize = false;
            this.labelGestionar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.labelGestionar.ForeColor = System.Drawing.Color.FromArgb(109, 91, 208);
            this.labelGestionar.Location = new System.Drawing.Point(0, 8);
            this.labelGestionar.Size = new System.Drawing.Size(120, 20);
            this.labelGestionar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGestionar.Text = "✎  Gestionar";
            this.labelGestionar.BackColor = System.Drawing.Color.Transparent;
            this.labelGestionar.Click += new System.EventHandler(this.btnGestionar_Click);
            this.btnGestionar.Controls.Add(this.labelGestionar);

            // --- Favoritos panel ---
            this.panelFavoritos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFavoritos.BackColor = System.Drawing.Color.White;
            this.panelFavoritos.BorderColor = System.Drawing.Color.FromArgb(235, 236, 242);
            this.panelFavoritos.BorderThickness = 1;
            this.panelFavoritos.CornerRadius = 16;
            this.panelFavoritos.Location = new System.Drawing.Point(32, 446);
            this.panelFavoritos.Size = new System.Drawing.Size(1086, 236);

            this.panelFavCards.Location = new System.Drawing.Point(24, 20);
            this.panelFavCards.Size = new System.Drawing.Size(1038, 196);
            this.panelFavCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFavCards.BackColor = System.Drawing.Color.Transparent;

            this.panelFavoritos.Controls.Add(this.panelFavCards);

            // --- Banner de consejo ---
            this.panelTip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTip.BackColor = System.Drawing.Color.FromArgb(232, 243, 255);
            this.panelTip.CornerRadius = 12;
            this.panelTip.Location = new System.Drawing.Point(32, 698);
            this.panelTip.Size = new System.Drawing.Size(1086, 56);

            this.pictureBulb.Location = new System.Drawing.Point(20, 14);
            this.pictureBulb.Size = new System.Drawing.Size(24, 24);
            this.pictureBulb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBulb.BackColor = System.Drawing.Color.Transparent;
            this.pictureBulb.TabStop = false;

            this.labelTip.AutoSize = true;
            this.labelTip.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelTip.ForeColor = System.Drawing.Color.FromArgb(40, 60, 90);
            this.labelTip.Location = new System.Drawing.Point(56, 18);
            this.labelTip.Text = "Consejo:  Arrastra y suelta archivos o carpetas en cualquier herramienta para comenzar rápidamente.";

            this.btnCerrarTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrarTip.Location = new System.Drawing.Point(1050, 19);
            this.btnCerrarTip.Size = new System.Drawing.Size(16, 16);
            this.btnCerrarTip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCerrarTip.BackColor = System.Drawing.Color.Transparent;
            this.btnCerrarTip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarTip.TabStop = false;
            this.btnCerrarTip.Click += new System.EventHandler(this.btnCerrarTip_Click);

            this.panelTip.Controls.Add(this.pictureBulb);
            this.panelTip.Controls.Add(this.labelTip);
            this.panelTip.Controls.Add(this.btnCerrarTip);

            this.panelContent.Controls.Add(this.panelTip);
            this.panelContent.Controls.Add(this.panelFavoritos);
            this.panelContent.Controls.Add(this.btnGestionar);
            this.panelContent.Controls.Add(this.labelFavHeader);
            this.panelContent.Controls.Add(this.panelRecent);
            this.panelContent.Controls.Add(this.btnBell);
            this.panelContent.Controls.Add(this.panelSearch);
            this.panelContent.Controls.Add(this.labelSubtitle);
            this.panelContent.Controls.Add(this.labelWelcome);

            // =========================================================
            // FORM
            // =========================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1450, 900);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.BackColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Principal";
            this.Load += new System.EventHandler(this.Principal_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Principal_FormClosed);

            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureClock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBulb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrarTip)).EndInit();
            this.ResumeLayout(true);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelSidebarDivider;
        private System.Windows.Forms.PictureBox pictureLogo;
        private System.Windows.Forms.Label labelAppTitle;
        private ConvertidorImagenes.RoundedPanel btnInicio;
        private System.Windows.Forms.PictureBox pictureHome;
        private System.Windows.Forms.Label labelInicio;
        private System.Windows.Forms.Label labelHerramientas;
        private ConvertidorImagenes.NavButton navConvertidor;
        private ConvertidorImagenes.NavButton navOffice;
        private ConvertidorImagenes.NavButton navOcr;
        private ConvertidorImagenes.NavButton navResize;
        private ConvertidorImagenes.NavButton navPlantillas;
        private ConvertidorImagenes.NavButton navAyuda;
        private System.Windows.Forms.Panel dividerNav;
        private System.Windows.Forms.Label labelFavoritosNav;
        private System.Windows.Forms.Label labelVersion;

        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Label labelSubtitle;
        private ConvertidorImagenes.RoundedPanel panelSearch;
        private System.Windows.Forms.PictureBox pictureSearch;
        private System.Windows.Forms.TextBox txtBuscar;
        private ConvertidorImagenes.RoundedPanel btnBell;
        private System.Windows.Forms.PictureBox pictureBell;

        private ConvertidorImagenes.RoundedPanel panelRecent;
        private System.Windows.Forms.PictureBox pictureClock;
        private System.Windows.Forms.Label labelRecentHeader;
        private System.Windows.Forms.Panel panelRecentCards;

        private System.Windows.Forms.Label labelFavHeader;
        private ConvertidorImagenes.RoundedPanel btnGestionar;
        private System.Windows.Forms.Label labelGestionar;
        private ConvertidorImagenes.RoundedPanel panelFavoritos;
        private System.Windows.Forms.Panel panelFavCards;

        private ConvertidorImagenes.RoundedPanel panelTip;
        private System.Windows.Forms.PictureBox pictureBulb;
        private System.Windows.Forms.Label labelTip;
        private System.Windows.Forms.PictureBox btnCerrarTip;
    }
}

