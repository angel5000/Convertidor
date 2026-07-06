using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ConvertidorImagenes
{
    public partial class Plantillas : Form
    {
        private TextBox txtDescripcion;
        private Label lblContador;
        private Button btnGenerarIA;
        private FlowLayoutPanel panelSugerencias;
        private RichTextBox rtbEditorPlantilla;
        private DataGridView dgvCampos;
        private Button btnAgregarCampo;
        private Button btnEliminarCampo;
        private Button btnGuardarPlantilla;
        private TextBox txtBuscarPlantilla;
        private ListBox lstPlantillas;
        private Button btnNuevaPlantillaIA;
        private Button btnCambiarCarpeta;
        private FlowLayoutPanel panelInputs;
        private WebBrowser webVistaPrevia;
        private Button btnExportarDocx;
        private Button btnExportar;
        private ComboBox cbFormatoExportar;
        private Button btnCargar;
        private Label lblEstado;

        // Nuevos campos para PestaÃ±as y Plantillas de Documentos
        private Panel panelSimples;
        private Panel panelDocumentos;
        private Button btnTabSimples;
        private Button btnTabDocumentos;

        private TextBox txtDocPrompt;
        private Label lblDocContador;
        private FlowLayoutPanel flpDocSugerencias;
        private Button btnDocGenerar;
        private FlowLayoutPanel flpDocInputs;
        private WebBrowser webDocPreview;
        private Button btnDocGuardar;
        private Button btnDocCargar;

        private string rutaPlantillas;
        private PlantillaModel plantillaActual;
        private Dictionary<string, Control> controlesDinamicos;
        private BindingList<CampoPlantilla> camposEditables;
        private GroqService groqService = new GroqService(ApiKeys.GroqApiKey);
        private GroqDocService groqDocService = new GroqDocService(ApiKeys.GroqApiKey);

        private readonly Color primaryColor = Color.FromArgb(103, 80, 164);
        private readonly Color primaryLight = Color.FromArgb(237, 231, 246);
        private readonly Color bgColor = Color.FromArgb(245, 246, 250);
        private readonly Color cardColor = Color.White;
        private readonly Color textDark = Color.FromArgb(33, 37, 41);
        private readonly Color textMuted = Color.FromArgb(130, 130, 140);
        private readonly Color borderLight = Color.FromArgb(222, 226, 230);
        private readonly Color successGreen = Color.FromArgb(25, 135, 84);

        public Plantillas()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            controlesDinamicos = new Dictionary<string, Control>();
            camposEditables = new BindingList<CampoPlantilla>();
            ConfigurarRutaBase();
            ConstruirUI();
            CargarListaPlantillas();
        }

        private void ConfigurarRutaBase()
        {
            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            rutaPlantillas = Path.Combine(docs, "ConvertidorImagenes", "Plantillas");
            if (!Directory.Exists(rutaPlantillas))
            {
                Directory.CreateDirectory(rutaPlantillas);
                GenerarPlantillasPorDefecto();
            }
            else
            {
                string[] archivos = Directory.GetFiles(rutaPlantillas, "*.json");
                if (archivos.Length == 0) GenerarPlantillasPorDefecto();
            }
        }

        private void GenerarPlantillasPorDefecto()
        {
            var p1 = new PlantillaModel { Titulo = "Contrato Laboral", ContenidoBase = "CONTRATO DE TRABAJO\n\nConste por el presente documento el contrato de trabajo que celebran de una parte [EMPRESA], y de la otra parte el señor(a) [NOMBRE].\n\nEl empleado desempeñará el cargo de [CARGO] a partir del [FECHA].\n\nLa remuneración mensual pactada es de [MONTO].\n\nAmbas partes firman en conformidad.\n\n______________________\n[EMPRESA]\n\n______________________\n[NOMBRE]" };
            p1.Campos.Add(new CampoPlantilla { Id = "[NOMBRE]", Etiqueta = "Nombre Completo", Tipo = "Texto" });
            p1.Campos.Add(new CampoPlantilla { Id = "[EMPRESA]", Etiqueta = "Empresa", Tipo = "Texto" });
            p1.Campos.Add(new CampoPlantilla { Id = "[CARGO]", Etiqueta = "Cargo", Tipo = "Texto" });
            p1.Campos.Add(new CampoPlantilla { Id = "[FECHA]", Etiqueta = "Fecha", Tipo = "Fecha" });
            p1.Campos.Add(new CampoPlantilla { Id = "[MONTO]", Etiqueta = "Monto", Tipo = "Monto" });

            var p2 = new PlantillaModel { Titulo = "Carta de Renuncia", ContenidoBase = "CARTA DE RENUNCIA\n\n[FECHA]\n\nSeñores,\n[EMPRESA]\n\nPor la presente, comunico mi renuncia irrevocable al cargo de [CARGO].\n\nAgradezco las oportunidades de crecimiento.\n\nAtentamente,\n\n______________________\n[NOMBRE]" };
            p2.Campos.Add(new CampoPlantilla { Id = "[NOMBRE]", Etiqueta = "Tu Nombre", Tipo = "Texto" });
            p2.Campos.Add(new CampoPlantilla { Id = "[EMPRESA]", Etiqueta = "Empresa", Tipo = "Texto" });
            p2.Campos.Add(new CampoPlantilla { Id = "[CARGO]", Etiqueta = "Cargo", Tipo = "Texto" });
            p2.Campos.Add(new CampoPlantilla { Id = "[FECHA]", Etiqueta = "Fecha", Tipo = "Fecha" });

            var p3 = new PlantillaModel { Titulo = "Carta de inasistencia por salud", ContenidoBase = "Estimado/a [NOMBRE_JEFE]:\n\nPor medio de la presente, le informo que debido a un problema de salud no me será posible asistir a mis labores el día de hoy.\n\nMe encuentro siguiendo las indicaciones médicas para mi recuperación y, de ser necesario, haré llegar el justificante o certificado médico correspondiente.\n\nAgradezco su comprensión.\n\nSaludos cordiales,\n\n[NOMBRE_EMPLEADO]" };
            p3.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_JEFE]", Etiqueta = "Nombre del jefe", Tipo = "Texto" });
            p3.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_EMPLEADO]", Etiqueta = "Nombre del empleado", Tipo = "Texto" });

            var p4 = new PlantillaModel { Titulo = "Carta de recomendacion laboral", ContenidoBase = "CARTA DE RECOMENDACIÓN\n\n[FECHA]\n\nA quien corresponda:\n\nPor medio de la presente hago constar que el/la Sr(a). [NOMBRE_EMPLEADO] laboró en [EMPRESA] durante el periodo del [FECHA_INICIO] al [FECHA_FIN], desempeñando el cargo de [CARGO].\n\nDurante su permanencia demostró excelente desempeño profesional.\n\nAtentamente,\n\n______________________\n[NOMBRE_JEFE]\n[CARGO_JEFE]\n[EMPRESA]" };
            p4.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_EMPLEADO]", Etiqueta = "Nombre del empleado", Tipo = "Texto" });
            p4.Campos.Add(new CampoPlantilla { Id = "[EMPRESA]", Etiqueta = "Empresa", Tipo = "Texto" });
            p4.Campos.Add(new CampoPlantilla { Id = "[CARGO]", Etiqueta = "Cargo del empleado", Tipo = "Texto" });
            p4.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_JEFE]", Etiqueta = "Nombre del jefe", Tipo = "Texto" });
            p4.Campos.Add(new CampoPlantilla { Id = "[CARGO_JEFE]", Etiqueta = "Cargo del jefe", Tipo = "Texto" });
            p4.Campos.Add(new CampoPlantilla { Id = "[FECHA]", Etiqueta = "Fecha actual", Tipo = "Fecha" });
            p4.Campos.Add(new CampoPlantilla { Id = "[FECHA_INICIO]", Etiqueta = "Fecha de inicio", Tipo = "Fecha" });
            p4.Campos.Add(new CampoPlantilla { Id = "[FECHA_FIN]", Etiqueta = "Fecha de fin", Tipo = "Fecha" });

            var p5 = new PlantillaModel { Titulo = "Contrato de Alquiler", ContenidoBase = "CONTRATO DE ARRENDAMIENTO\n\nEn la ciudad de [CIUDAD], a [FECHA], comparecen:\n\nARRENDADOR: [NOMBRE_ARRENDADOR]\nARRENDATARIO: [NOMBRE_ARRENDATARIO]\n\nRenta mensual: [MONTO].\nDireccion: [DIRECCION].\nDuracion: [DURACION].\n\n______________________\n[NOMBRE_ARRENDADOR]\nArrendador\n\n______________________\n[NOMBRE_ARRENDATARIO]\nArrendatario" };
            p5.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_ARRENDADOR]", Etiqueta = "Arrendador", Tipo = "Texto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[NOMBRE_ARRENDATARIO]", Etiqueta = "Arrendatario", Tipo = "Texto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[CIUDAD]", Etiqueta = "Ciudad", Tipo = "Texto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[DIRECCION]", Etiqueta = "Direccion", Tipo = "Texto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[DURACION]", Etiqueta = "Duracion", Tipo = "Texto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[MONTO]", Etiqueta = "Renta mensual", Tipo = "Monto" });
            p5.Campos.Add(new CampoPlantilla { Id = "[FECHA]", Etiqueta = "Fecha del contrato", Tipo = "Fecha" });

            File.WriteAllText(Path.Combine(rutaPlantillas, "Contrato_Laboral.json"), JsonConvert.SerializeObject(p1, Formatting.Indented));
            File.WriteAllText(Path.Combine(rutaPlantillas, "Carta_de_Renuncia.json"), JsonConvert.SerializeObject(p2, Formatting.Indented));
            File.WriteAllText(Path.Combine(rutaPlantillas, "Carta_de_inasistencia_por_salud.json"), JsonConvert.SerializeObject(p3, Formatting.Indented));
            File.WriteAllText(Path.Combine(rutaPlantillas, "Carta_de_recomendacion_laboral.json"), JsonConvert.SerializeObject(p4, Formatting.Indented));
            File.WriteAllText(Path.Combine(rutaPlantillas, "Contrato_de_Alquiler.json"), JsonConvert.SerializeObject(p5, Formatting.Indented));
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void DragForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void ConstruirUI()
        {
            this.ClientSize = new Size(1280, 830);
            this.BackColor = bgColor;
            this.Text = "IA para Generar Plantillas";
            this.AutoScroll = true;
            this.MouseDown += new MouseEventHandler(DragForm_MouseDown);

            Font titleFont = new Font("Segoe UI", 17, FontStyle.Bold);
            Font subtitleFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font smallBold = new Font("Segoe UI", 9f, FontStyle.Bold);
            Font regularFont = new Font("Segoe UI", 9.5f);
            Font smallFont = new Font("Segoe UI", 8.5f);
            int marginX = 30;
            int topY = 10;

            // HEADER TABS
            Panel tabContainer = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.White };
            this.Controls.Add(tabContainer);

            btnTabSimples = new Button { Text = "Plantillas Simples", Dock = DockStyle.Left, Width = 250, FlatStyle = FlatStyle.Flat, BackColor = primaryColor, ForeColor = Color.White, Font = subtitleFont, Cursor = Cursors.Hand };
            btnTabSimples.FlatAppearance.BorderSize = 0;
            btnTabSimples.Click += (s, e) => MostrarTab(0);
            tabContainer.Controls.Add(btnTabSimples);

            btnTabDocumentos = new Button { Text = "Plantillas de Documentos", Dock = DockStyle.Left, Width = 300, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = textMuted, Font = subtitleFont, Cursor = Cursors.Hand };
            btnTabDocumentos.FlatAppearance.BorderSize = 0;
            btnTabDocumentos.Click += (s, e) => MostrarTab(1);
            tabContainer.Controls.Add(btnTabDocumentos);

            Panel separador = new Panel { Dock = DockStyle.Bottom, Height = 2, BackColor = primaryColor };
            tabContainer.Controls.Add(separador);

            // CONTENEDORES PRINCIPALES
            panelSimples = new Panel { Dock = DockStyle.Fill, BackColor = bgColor };
            this.Controls.Add(panelSimples);

            panelDocumentos = new Panel { Dock = DockStyle.Fill, BackColor = bgColor, Visible = false };
            this.Controls.Add(panelDocumentos);

            // --- TAB 1: PLANTILLAS SIMPLES ---
            Label lblTitle = new Label { Text = "IA para Generar Plantillas", Font = titleFont, ForeColor = textDark, AutoSize = true, Location = new Point(marginX, topY) };
            lblTitle.MouseDown += DragForm_MouseDown;
            panelSimples.Controls.Add(lblTitle);
            Label lblSubTitle = new Label { Text = "Describe el documento que necesitas y la IA generará una plantilla lista para usar.", Font = smallFont, ForeColor = textMuted, AutoSize = true, Location = new Point(marginX, topY + 30) };
            panelSimples.Controls.Add(lblSubTitle);

            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Location = new Point(marginX, topY + 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(this.ClientSize.Width - (marginX * 2), this.ClientSize.Height - (topY + 60) - 20 - 55),
                ColumnCount = 3,
                RowCount = 3
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            tlp.SuspendLayout();
            panelSimples.Controls.Add(tlp);

            // COL 1: Describe
            RoundedPanel pnl1 = new RoundedPanel { Size = new Size(1000, 1000), BackColor = cardColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 15, 15), BorderColor = borderLight, BorderThickness = 1 };
            pnl1.MouseDown += DragForm_MouseDown;
            tlp.Controls.Add(pnl1, 0, 0);
            tlp.SetRowSpan(pnl1, 2);

            pnl1.Controls.Add(new Label { Text = "1", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = Color.White, BackColor = primaryColor, Size = new Size(20, 20), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(15, 12) });
            pnl1.Controls.Add(new Label { Text = "Describe tu plantilla", Font = subtitleFont, ForeColor = textDark, Location = new Point(40, 10), AutoSize = true });
            pnl1.Controls.Add(new Label { Text = "Cuéntale a la IA qué documento necesitas.", Font = smallFont, ForeColor = textMuted, Location = new Point(15, 32), AutoSize = true });

            txtDescripcion = new TextBox { Location = new Point(15, 55), Size = new Size(pnl1.Width - 30, pnl1.Height - 200), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, Font = regularFont, Multiline = true, ScrollBars = ScrollBars.Vertical };
            pnl1.Controls.Add(txtDescripcion);

            lblContador = new Label { Text = "0/500", Font = smallFont, ForeColor = textMuted, Location = new Point(pnl1.Width - 60, pnl1.Height - 90), Anchor = AnchorStyles.Bottom | AnchorStyles.Right, AutoSize = true };
            pnl1.Controls.Add(lblContador);
            txtDescripcion.TextChanged += (s, ev) => { lblContador.Text = txtDescripcion.Text.Length + "/500"; };

            pnl1.Controls.Add(new Label { Text = "Sugerencias:", Font = smallBold, ForeColor = textMuted, Location = new Point(15, pnl1.Height - 120), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, AutoSize = true });
            panelSugerencias = new FlowLayoutPanel { Location = new Point(15, pnl1.Height - 90), Size = new Size(pnl1.Width - 30, 35), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, WrapContents = true, AutoScroll = true };
            pnl1.Controls.Add(panelSugerencias);
            string[] sugs = { "Carta de recomendación", "Solicitud de empleo", "Contrato de alquiler", "Notificación de pago" };
            foreach (string s in sugs)
            {
                Button chip = new Button { Text = s, Font = smallFont, ForeColor = textDark, BackColor = Color.FromArgb(243, 244, 246), FlatStyle = FlatStyle.Flat, AutoSize = true, Margin = new Padding(2), Padding = new Padding(6, 2, 6, 2), Cursor = Cursors.Hand };
                chip.FlatAppearance.BorderColor = borderLight;
                chip.Click += (se, ev) => { txtDescripcion.Text = s; };
                panelSugerencias.Controls.Add(chip);
            }
            btnGenerarIA = new Button { Text = "Generar plantilla con IA", Location = new Point(15, pnl1.Height - 50), Size = new Size(pnl1.Width - 30, 35), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = primaryColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = smallBold, Cursor = Cursors.Hand };
            btnGenerarIA.FlatAppearance.BorderSize = 0;
            btnGenerarIA.Click += BtnGenerarIA_Click;
            pnl1.Controls.Add(btnGenerarIA);

            // COL 2: Revisa y edita
            RoundedPanel pnl2 = new RoundedPanel { Size = new Size(1000, 1000), BackColor = cardColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 15, 15), BorderColor = borderLight, BorderThickness = 1 };
            pnl2.MouseDown += DragForm_MouseDown;
            tlp.Controls.Add(pnl2, 1, 0);

            pnl2.Controls.Add(new Label { Text = "2", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = Color.White, BackColor = primaryColor, Size = new Size(20, 20), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(15, 12) });
            pnl2.Controls.Add(new Label { Text = "Revisa y edita la plantilla generada", Font = subtitleFont, ForeColor = textDark, Location = new Point(40, 10), AutoSize = true });
            pnl2.Controls.Add(new Label { Text = "Puedes editar el texto directamente.", Font = smallFont, ForeColor = textMuted, Location = new Point(15, 32), AutoSize = true });

            rtbEditorPlantilla = new RichTextBox { Location = new Point(15, 55), Size = new Size(pnl2.Width - 30, pnl2.Height - 120), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Consolas", 9.5f), BorderStyle = System.Windows.Forms.BorderStyle.None, BackColor = Color.FromArgb(250, 250, 252) };
            pnl2.Controls.Add(rtbEditorPlantilla);

            pnl2.Controls.Add(new Label { Text = "Mejorar con IA:", Font = smallBold, ForeColor = textMuted, Location = new Point(15, pnl2.Height - 55), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, AutoSize = true });

            FlowLayoutPanel panelEstilos = new FlowLayoutPanel { Location = new Point(15, pnl2.Height - 35), Size = new Size(pnl2.Width - 30, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, WrapContents = false, AutoScroll = true };
            pnl2.Controls.Add(panelEstilos);

            var estilos = new (string texto, string prompt)[] {
                ("🏛️ Institucional", "Haz que parezca un comunicado oficial institucional."),
                ("🏦 Bancario", "Haz que parezca un documento oficial emitido por una entidad bancaria, usando lenguaje financiero."),
                ("⚖️ Jurídico", "Haz que parezca un contrato o documento legal elaborado por un abogado."),
                ("👔 Empresarial", "Haz que tenga un tono altamente profesional y corporativo."),
                ("📄 Administrativo", "Haz que tenga un tono formal administrativo para trámites o RRHH."),
                ("🤝 Comercial", "Haz que tenga un tono comercial y persuasivo, orientado a clientes o ventas."),
                ("😊 Cercano", "Haz que tenga un tono cordial, amable y cercano."),
                ("🌍 Traducir", "Traduce todo el documento al idioma inglés de manera nativa y fluida.")
            };

            foreach (var est in estilos)
            {
                Button btnEstilo = new Button { Text = est.texto, Tag = est.prompt, Font = smallFont, ForeColor = textDark, BackColor = Color.FromArgb(243, 244, 246), FlatStyle = FlatStyle.Flat, AutoSize = true, Margin = new Padding(2, 0, 4, 0), Padding = new Padding(6, 2, 6, 2), Cursor = Cursors.Hand };
                btnEstilo.FlatAppearance.BorderColor = borderLight;
                btnEstilo.Click += BtnEstilo_Click;
                panelEstilos.Controls.Add(btnEstilo);
            }

            // COL 3: Campos detectados
            RoundedPanel pnl3 = new RoundedPanel { Size = new Size(1000, 1000), BackColor = cardColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 15), BorderColor = borderLight, BorderThickness = 1 };
            pnl3.MouseDown += DragForm_MouseDown;
            tlp.Controls.Add(pnl3, 2, 0);

            pnl3.Controls.Add(new Label { Text = "3", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = Color.White, BackColor = primaryColor, Size = new Size(20, 20), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(15, 12) });
            pnl3.Controls.Add(new Label { Text = "Campos detectados", Font = subtitleFont, ForeColor = textDark, Location = new Point(40, 10), AutoSize = true });
            pnl3.Controls.Add(new Label { Text = "Campos que la IA identifico en la plantilla.", Font = smallFont, ForeColor = textMuted, Location = new Point(15, 32), AutoSize = true });

            btnAgregarCampo = new Button { Text = "+ Agregar campo", Location = new Point(pnl3.Width - 195, 52), Size = new Size(115, 28), Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.White, ForeColor = textDark, FlatStyle = FlatStyle.Flat, Font = smallFont, Cursor = Cursors.Hand };
            btnAgregarCampo.FlatAppearance.BorderColor = borderLight;
            btnAgregarCampo.Click += BtnAgregarCampo_Click;
            pnl3.Controls.Add(btnAgregarCampo);

            btnEliminarCampo = new Button { Text = "X", Location = new Point(pnl3.Width - 70, 52), Size = new Size(40, 28), Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.White, ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Font = regularFont, Cursor = Cursors.Hand };
            btnEliminarCampo.FlatAppearance.BorderColor = borderLight;
            btnEliminarCampo.Click += BtnEliminarCampo_Click;
            pnl3.Controls.Add(btnEliminarCampo);

            dgvCampos = new DataGridView { Location = new Point(15, 85), Size = new Size(pnl3.Width - 30, pnl3.Height - 150), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, AllowUserToAddRows = false, AllowUserToDeleteRows = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = System.Windows.Forms.BorderStyle.None, Font = smallFont, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = borderLight, ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single };
            dgvCampos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvCampos.ColumnHeadersDefaultCellStyle.ForeColor = textMuted;
            dgvCampos.ColumnHeadersDefaultCellStyle.Font = smallBold;
            dgvCampos.EnableHeadersVisualStyles = false;
            dgvCampos.DefaultCellStyle.SelectionBackColor = primaryLight;
            dgvCampos.DefaultCellStyle.SelectionForeColor = textDark;
            pnl3.Controls.Add(dgvCampos);

            btnGuardarPlantilla = new Button { Text = "Guardar plantilla", Location = new Point(15, pnl3.Height - 55), Size = new Size(pnl3.Width - 30, 42), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = successGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGuardarPlantilla.FlatAppearance.BorderSize = 0;
            btnGuardarPlantilla.Click += BtnGuardarPlantilla_Click;
            pnl3.Controls.Add(btnGuardarPlantilla);

            // BOTTOM ROW - Mis Plantillas
            RoundedPanel pnl4 = new RoundedPanel { Size = new Size(1000, 1000), BackColor = cardColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 15, 0), BorderColor = borderLight, BorderThickness = 1 };
            pnl4.MouseDown += DragForm_MouseDown;
            tlp.Controls.Add(pnl4, 0, 2);

            pnl4.Controls.Add(new Label { Text = "Mis Plantillas", Font = subtitleFont, ForeColor = textDark, Location = new Point(15, 12), AutoSize = true });
            pnl4.Controls.Add(new Label { Text = "Administra tus plantillas", Font = smallFont, ForeColor = textMuted, Location = new Point(15, 32), AutoSize = true });

            txtBuscarPlantilla = new TextBox { Location = new Point(15, 55), Size = new Size(pnl4.Width - 30, 25), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = regularFont };
            txtBuscarPlantilla.TextChanged += TxtBuscarPlantilla_TextChanged;
            pnl4.Controls.Add(txtBuscarPlantilla);

            lstPlantillas = new ListBox { Location = new Point(15, 85), Size = new Size(pnl4.Width - 30, pnl4.Height - 170), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, Font = regularFont, BorderStyle = System.Windows.Forms.BorderStyle.None, BackColor = Color.White, IntegralHeight = false };
            lstPlantillas.DrawMode = DrawMode.OwnerDrawFixed;
            lstPlantillas.ItemHeight = 52;
            lstPlantillas.DrawItem += LstPlantillas_DrawItem;
            lstPlantillas.SelectedIndexChanged += LstPlantillas_SelectedIndexChanged;
            pnl4.Controls.Add(lstPlantillas);

            btnCambiarCarpeta = new Button { Text = "Cambiar carpeta", Location = new Point(15, pnl4.Height - 80), Size = new Size(pnl4.Width - 30, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.FromArgb(243, 244, 246), ForeColor = textDark, FlatStyle = FlatStyle.Flat, Font = smallFont, Cursor = Cursors.Hand };
            btnCambiarCarpeta.FlatAppearance.BorderColor = borderLight;
            btnCambiarCarpeta.Click += BtnCambiarCarpeta_Click;
            pnl4.Controls.Add(btnCambiarCarpeta);

            btnNuevaPlantillaIA = new Button { Text = "+ Nueva plantilla IA", Location = new Point(15, pnl4.Height - 42), Size = new Size(pnl4.Width - 30, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White, ForeColor = primaryColor, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnNuevaPlantillaIA.FlatAppearance.BorderColor = primaryColor;
            btnNuevaPlantillaIA.Click += BtnNuevaPlantillaIA_Click;
            pnl4.Controls.Add(btnNuevaPlantillaIA);

            // Vista previa (Span 2 cols y 2 rows)
            RoundedPanel pnl5 = new RoundedPanel { Size = new Size(1000, 1000), BackColor = cardColor, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 0), BorderColor = borderLight, BorderThickness = 1 };
            pnl5.MouseDown += DragForm_MouseDown;
            tlp.Controls.Add(pnl5, 1, 1);
            tlp.SetColumnSpan(pnl5, 2);
            tlp.SetRowSpan(pnl5, 2);

            pnl5.Controls.Add(new Label { Text = "Vista previa de la plantilla", Font = subtitleFont, ForeColor = textDark, Location = new Point(15, 12), AutoSize = true });
            pnl5.Controls.Add(new Label { Text = "Asi se vera el documento al completar los campos.", Font = smallFont, ForeColor = textMuted, Location = new Point(15, 32), AutoSize = true });

            panelInputs = new FlowLayoutPanel { Location = new Point(15, 55), Size = new Size(360, pnl5.Height - 120), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            pnl5.Controls.Add(panelInputs);

            webVistaPrevia = new WebBrowser { Location = new Point(390, 55), Size = new Size(pnl5.Width - 410, pnl5.Height - 120), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnl5.Controls.Add(webVistaPrevia);

            btnExportarDocx = new Button { Text = "Exportar a DOCX", Location = new Point(15, pnl5.Height - 55), Size = new Size(160, 42), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, BackColor = primaryColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnExportarDocx.FlatAppearance.BorderSize = 0;
            btnExportarDocx.Click += BtnExportarDocx_Click;
            pnl5.Controls.Add(btnExportarDocx);

            cbFormatoExportar = new ComboBox { Location = new Point(185, pnl5.Height - 48), Size = new Size(70, 42), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11f) };
            cbFormatoExportar.Items.AddRange(new string[] { "TXT", "XML" });
            cbFormatoExportar.SelectedIndex = 0;
            pnl5.Controls.Add(cbFormatoExportar);

            btnExportar = new Button { Text = "Guardar", Location = new Point(265, pnl5.Height - 55), Size = new Size(90, 38), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, BackColor = Color.White, ForeColor = textDark, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnExportar.FlatAppearance.BorderColor = borderLight;
            btnExportar.Click += BtnExportar_Click;
            pnl5.Controls.Add(btnExportar);

            btnCargar = new Button { Text = "Cargar", Location = new Point(365, pnl5.Height - 55), Size = new Size(90, 38), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, BackColor = Color.White, ForeColor = primaryColor, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCargar.FlatAppearance.BorderColor = primaryColor;
            btnCargar.Click += BtnCargar_Click;
            pnl5.Controls.Add(btnCargar);

            lblEstado = new Label { Text = "", Font = regularFont, ForeColor = successGreen, Location = new Point(465, pnl5.Height - 45), Anchor = AnchorStyles.Bottom | AnchorStyles.Left, AutoSize = true };
            pnl5.Controls.Add(lblEstado);

            tlp.ResumeLayout(false);

            // --- TAB 2: PLANTILLAS DE DOCUMENTOS ---
            TableLayoutPanel tlpDoc = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10),
                ColumnCount = 2,
                RowCount = 1
            };
            tlpDoc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            tlpDoc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            panelDocumentos.Controls.Add(tlpDoc);

            // LEFT SIDE: SECCION IA & INPUTS
            TableLayoutPanel tlpDocLeft = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            tlpDocLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 410f));
            tlpDocLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 190f));
            tlpDocLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 85f));
            tlpDoc.Controls.Add(tlpDocLeft, 0, 0);

            // Left 1: IA
            RoundedPanel pnlDocIa = new RoundedPanel { Dock = DockStyle.Fill, BackColor = cardColor, Margin = new Padding(0, 0, 5, 8), BorderColor = borderLight, BorderThickness = 1, Padding = new Padding(10) };
            tlpDocLeft.Controls.Add(pnlDocIa, 0, 0);

            Panel pnlIaTop = new Panel { Dock = DockStyle.Top, Height = 70 };
            pnlIaTop.Controls.Add(new Label { Text = "📋 Sección de la IA", Font = smallBold, ForeColor = primaryColor, Location = new Point(0, 2), AutoSize = true });
            pnlDocIa.Controls.Add(pnlIaTop);

            Panel pnlIaBottom = new Panel { Dock = DockStyle.Bottom, Height = 80 };

            pnlIaBottom.Controls.Add(new Label { Text = "Sugerencias:", Font = smallBold, ForeColor = textMuted, Location = new Point(0, 0), AutoSize = true });
            flpDocSugerencias = new FlowLayoutPanel { Location = new Point(0, 20), Size = new Size(pnlIaBottom.Width, 30), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, WrapContents = false, AutoScroll = true };
            pnlIaBottom.Controls.Add(flpDocSugerencias);

            string[] docSugs = { "Carta de recomendación", "Solicitud de empleo", "Comprobante de pago" };
            foreach (string s in docSugs)
            {
                Button chip = new Button { Text = s, Font = smallFont, ForeColor = textDark, BackColor = Color.FromArgb(243, 244, 246), FlatStyle = FlatStyle.Flat, AutoSize = true, Margin = new Padding(2, 0, 4, 0), Cursor = Cursors.Hand };
                chip.FlatAppearance.BorderColor = borderLight;
                chip.Click += (se, ev) => { txtDocPrompt.Text = s; };
                flpDocSugerencias.Controls.Add(chip);
            }

            btnDocGenerar = new Button { Text = "✨ Generar plantilla con IA", Dock = DockStyle.Bottom, Height = 35, BackColor = primaryColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = smallBold, Cursor = Cursors.Hand };
            btnDocGenerar.FlatAppearance.BorderSize = 0;
            btnDocGenerar.Click += BtnDocGenerar_Click;
            pnlIaBottom.Controls.Add(btnDocGenerar);
            pnlDocIa.Controls.Add(pnlIaBottom);

            Panel pnlContador = new Panel { Dock = DockStyle.Bottom, Height = 20 };
            lblDocContador = new Label { Text = "0/500", Font = smallFont, ForeColor = textMuted, Dock = DockStyle.Right, AutoSize = true };
            pnlContador.Controls.Add(lblDocContador);
            pnlDocIa.Controls.Add(pnlContador);

            txtDocPrompt = new TextBox { Dock = DockStyle.Fill, Font = regularFont, Multiline = true, ScrollBars = ScrollBars.Vertical };
            txtDocPrompt.TextChanged += (s, ev) => { lblDocContador.Text = txtDocPrompt.Text.Length + "/500"; };
            pnlDocIa.Controls.Add(txtDocPrompt);
            txtDocPrompt.BringToFront();

            // Left 2: Inputs editables
            RoundedPanel pnlDocInputs = new RoundedPanel { Dock = DockStyle.Fill, BackColor = cardColor, Margin = new Padding(0, 0, 10, 8), BorderColor = borderLight, BorderThickness = 1, Padding = new Padding(10) };
            tlpDocLeft.Controls.Add(pnlDocInputs, 0, 1);

            Panel pnlInpTop = new Panel { Dock = DockStyle.Top, Height = 25 };
            pnlInpTop.Controls.Add(new Label { Text = "✏️ Inputs editables", Font = smallBold, ForeColor = primaryColor, Location = new Point(0, 2), AutoSize = true });
            pnlDocInputs.Controls.Add(pnlInpTop);

            flpDocInputs = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            pnlDocInputs.Controls.Add(flpDocInputs);
            flpDocInputs.BringToFront();

            Button btnAddInput = new Button { Text = "+ Agregar campo", Size = new Size(300, 30), BackColor = Color.White, FlatStyle = FlatStyle.Flat, ForeColor = textDark, Font = smallFont, Cursor = Cursors.Hand, Margin = new Padding(5) };
            btnAddInput.FlatAppearance.BorderColor = borderLight;
            btnAddInput.Click += BtnAddDocInput_Click;
            flpDocInputs.Controls.Add(btnAddInput);

            // Left 3: Acciones
            RoundedPanel pnlDocAcciones = new RoundedPanel { Dock = DockStyle.Fill, BackColor = cardColor, Margin = new Padding(0, 0, 10, 0), BorderColor = borderLight, BorderThickness = 1, Padding = new Padding(10) };
            tlpDocLeft.Controls.Add(pnlDocAcciones, 0, 2);

            Panel pnlAccTop = new Panel { Dock = DockStyle.Top, Height = 22 };
            pnlAccTop.Controls.Add(new Label { Text = "⚡ Acciones", Font = smallBold, ForeColor = primaryColor, Location = new Point(0, 2), AutoSize = true });
            pnlDocAcciones.Controls.Add(pnlAccTop);

            TableLayoutPanel tlpBtn = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlpBtn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpBtn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnDocGuardar = new Button { Text = "💾 Guardar plantilla", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 5, 0), BackColor = primaryColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = smallBold, Cursor = Cursors.Hand };
            btnDocGuardar.FlatAppearance.BorderSize = 0;
            btnDocGuardar.Click += BtnDocGuardar_Click;
            tlpBtn.Controls.Add(btnDocGuardar, 0, 0);

            btnDocCargar = new Button { Text = "📂 Cargar plantilla", Dock = DockStyle.Fill, Margin = new Padding(5, 0, 0, 0), BackColor = Color.White, ForeColor = textDark, FlatStyle = FlatStyle.Flat, Font = smallBold, Cursor = Cursors.Hand };
            btnDocCargar.FlatAppearance.BorderColor = borderLight;
            tlpBtn.Controls.Add(btnDocCargar, 1, 0);

            pnlDocAcciones.Controls.Add(tlpBtn);
            tlpBtn.BringToFront();

            // RIGHT SIDE: XML PREVIEW
            RoundedPanel pnlDocRight = new RoundedPanel { Dock = DockStyle.Fill, BackColor = cardColor, Margin = new Padding(0), BorderColor = borderLight, BorderThickness = 1, Padding = new Padding(15) };
            tlpDoc.Controls.Add(pnlDocRight, 1, 0);

            Panel pnlRightTop = new Panel { Dock = DockStyle.Top, Height = 50 };
            pnlRightTop.Controls.Add(new Label { Text = "ðŸ‘", Font = smallBold, ForeColor = primaryColor, Location = new Point(0, 0), AutoSize = true });
            pnlRightTop.Controls.Add(new Label { Text = "Visualizador del diseño del documento", Font = subtitleFont, ForeColor = textDark, Location = new Point(25, -3), AutoSize = true });
            pnlRightTop.Controls.Add(new Label { Text = "Vista previa basada en XML", Font = smallFont, ForeColor = textMuted, Location = new Point(25, 20), AutoSize = true });
            pnlDocRight.Controls.Add(pnlRightTop);

            Panel pnlZoom = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            pnlDocRight.Controls.Add(pnlZoom);

            Panel pnlZoomCenter = new Panel { Size = new Size(150, 40), Anchor = AnchorStyles.Top };
            pnlZoomCenter.Location = new Point((pnlZoom.Width - pnlZoomCenter.Width) / 2, 0);
            pnlZoom.Controls.Add(pnlZoomCenter);

            Button btnZoomOut = new Button { Text = "-", Location = new Point(0, 5), Size = new Size(30, 30), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnZoomOut.FlatAppearance.BorderColor = borderLight;
            btnZoomOut.Click += (s, e) => { SetWebZoom(zoomLevel - 20); };
            pnlZoomCenter.Controls.Add(btnZoomOut);

            Button btnZoom100 = new Button { Text = "100%", Location = new Point(40, 5), Size = new Size(60, 30), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnZoom100.FlatAppearance.BorderColor = borderLight;
            btnZoom100.Click += (s, e) => { SetWebZoom(100); };
            pnlZoomCenter.Controls.Add(btnZoom100);

            Button btnZoomIn = new Button { Text = "+", Location = new Point(110, 5), Size = new Size(30, 30), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnZoomIn.FlatAppearance.BorderColor = borderLight;
            btnZoomIn.Click += (s, e) => { SetWebZoom(zoomLevel + 20); };
            pnlZoomCenter.Controls.Add(btnZoomIn);

            Panel previewBg = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(243, 244, 246), Padding = new Padding(15) };
            webDocPreview = new WebBrowser { Dock = DockStyle.Fill, ScrollBarsEnabled = true };
            previewBg.Controls.Add(webDocPreview);
            pnlDocRight.Controls.Add(previewBg);
            previewBg.BringToFront();
        }

        private int zoomLevel = 100;
        private string templateRawXml = "";
        private PlantillaModel currentPlantillaModel;

        private void MostrarTab(int index)
        {
            if (index == 0)
            {
                panelSimples.Visible = true;
                panelDocumentos.Visible = false;
                btnTabSimples.BackColor = primaryColor;
                btnTabSimples.ForeColor = Color.White;
                btnTabDocumentos.BackColor = Color.White;
                btnTabDocumentos.ForeColor = textMuted;
            }
            else
            {
                panelSimples.Visible = false;
                panelDocumentos.Visible = true;
                btnTabDocumentos.BackColor = primaryColor;
                btnTabDocumentos.ForeColor = Color.White;
                btnTabSimples.BackColor = Color.White;
                btnTabSimples.ForeColor = textMuted;

                if (string.IsNullOrEmpty(templateRawXml))
                    UpdateWebPreview("<div>Escribe un prompt a la izquierda y presiona <b>Generar plantilla con IA</b> para comenzar.</div>");
            }
        }

        private void SetWebZoom(int zoom)
        {
            if (zoom < 20) zoom = 20;
            if (zoom > 300) zoom = 300;
            zoomLevel = zoom;
            if (webDocPreview.Document != null && webDocPreview.Document.Body != null)
            {
                webDocPreview.Document.Body.Style = $"zoom: {zoom}%;";
            }
        }

        private void UpdateWebPreview(string htmlContent)
        {
            string html = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: 'Segoe UI', Arial, sans-serif;
                        padding: 40px;
                        margin: 0;
                        background: white;
                        box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                        zoom: {zoomLevel}%;
                    }}
                    .placeholder {{ font-weight: bold; color: #6750A4; }}
                    h1, h2, h3 {{ text-align: center; color: #333; }}
                    hr {{ border: 0; height: 1px; background: #ddd; margin: 20px 0; }}
                    .firma {{ text-align: center; margin-top: 50px; }}
                </style>
            </head>
            <body>
                {htmlContent}
            </body>
            </html>";
            webDocPreview.DocumentText = html;
        }

        private void CargarListaPlantillas()
        {
            lstPlantillas.Items.Clear();
            if (Directory.Exists(rutaPlantillas))
            {
                string[] files = Directory.GetFiles(rutaPlantillas, "*.json");
                foreach (var f in files.OrderByDescending(x => File.GetLastWriteTime(x)))
                    lstPlantillas.Items.Add(Path.GetFileNameWithoutExtension(f));
                if (lstPlantillas.Items.Count > 0) lstPlantillas.SelectedIndex = 0;
            }
        }

        private void TxtBuscarPlantilla_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscarPlantilla.Text.Trim().ToLower();
            lstPlantillas.Items.Clear();
            if (Directory.Exists(rutaPlantillas))
            {
                string[] files = Directory.GetFiles(rutaPlantillas, "*.json");
                foreach (var f in files.OrderByDescending(x => File.GetLastWriteTime(x)))
                {
                    string name = Path.GetFileNameWithoutExtension(f);
                    if (string.IsNullOrEmpty(filtro) || name.ToLower().Contains(filtro))
                        lstPlantillas.Items.Add(name);
                }
            }
        }

        private void LstPlantillas_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            string nombre = lstPlantillas.Items[e.Index].ToString().Replace("_", " ");
            string archivo = Path.Combine(rutaPlantillas, lstPlantillas.Items[e.Index].ToString() + ".json");
            string fecha = File.Exists(archivo) ? "Actualizado: " + File.GetLastWriteTime(archivo).ToString("dd/MM/yyyy HH:mm") : "";
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color bg = selected ? primaryLight : cardColor;
            using (var bgBrush = new SolidBrush(bg))
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            if (selected)
            {
                using (var barBrush = new SolidBrush(primaryColor))
                    e.Graphics.FillRectangle(barBrush, e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height);
            }
            using (var titleBrush = new SolidBrush(textDark))
            using (var subBrush = new SolidBrush(textMuted))
            {
                e.Graphics.DrawString(nombre, new Font("Segoe UI", 9.5f, FontStyle.Bold), titleBrush, e.Bounds.X + 15, e.Bounds.Y + 6);
                e.Graphics.DrawString(fecha, new Font("Segoe UI", 8f), subBrush, e.Bounds.X + 15, e.Bounds.Y + 28);
            }
        }

        private int filasActuales = 1;
        private string lineaTablaOriginal = "";

        private void LstPlantillas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPlantillas.SelectedItem == null) return;
            string archivo = Path.Combine(rutaPlantillas, lstPlantillas.SelectedItem.ToString() + ".json");
            if (File.Exists(archivo))
            {
                string json = File.ReadAllText(archivo);
                plantillaActual = JsonConvert.DeserializeObject<PlantillaModel>(json);
                filasActuales = 1;
                lineaTablaOriginal = "";
                GenerarCamposDinamicos();
                ActualizarVistaPrevia();
            }
        }

        private List<string> AnalizarTablaExpansible()
        {
            List<string> variablesEnTabla = new List<string>();
            if (plantillaActual == null || string.IsNullOrWhiteSpace(plantillaActual.ContenidoBase)) return variablesEnTabla;

            string[] lines = plantillaActual.ContenidoBase.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("|") && line.Trim().EndsWith("|"))
                {
                    MatchCollection matches = Regex.Matches(line, @"\[([^\]]+)\]");
                    if (matches.Count > 0)
                    {
                        lineaTablaOriginal = line;
                        foreach (Match m in matches)
                        {
                            variablesEnTabla.Add("[" + m.Groups[1].Value + "]");
                        }
                        return variablesEnTabla;
                    }
                }
            }
            lineaTablaOriginal = "";
            return variablesEnTabla;
        }

        private void BtnAgregarFila_Click(object sender, EventArgs e)
        {
            if (filasActuales >= 5) return;

            List<string> vars = AnalizarTablaExpansible();
            if (vars.Count == 0 || string.IsNullOrEmpty(lineaTablaOriginal)) return;

            // Renombrar fila 1 si es la primera vez
            if (filasActuales == 1)
            {
                string nuevaLinea1 = lineaTablaOriginal;
                foreach (var v in vars)
                {
                    string baseName = v.Trim('[', ']');
                    nuevaLinea1 = nuevaLinea1.Replace(v, $"[{baseName}_1]");

                    var campoOrig = plantillaActual.Campos.FirstOrDefault(c => c.Id.Equals(v, StringComparison.OrdinalIgnoreCase));
                    if (campoOrig != null)
                    {
                        campoOrig.Id = $"[{baseName.ToUpper()}_1]";
                        if (!campoOrig.Etiqueta.EndsWith(" 1")) campoOrig.Etiqueta += " 1";
                    }
                }
                plantillaActual.ContenidoBase = plantillaActual.ContenidoBase.Replace(lineaTablaOriginal, nuevaLinea1);
                lineaTablaOriginal = nuevaLinea1;
            }

            filasActuales++;
            string nuevaLinea = lineaTablaOriginal;

            // Reemplazar los identificadores por el nuevo Ã­ndice de fila
            foreach (var v in vars)
            {
                string baseName = v.Trim('[', ']');
                // La linea original ya tiene _1 o _X porque se renombrÃ³ en la iteraciÃ³n previa
                nuevaLinea = nuevaLinea.Replace($"[{baseName}_{filasActuales - 1}]", $"[{baseName}_{filasActuales}]");

                var campoPrev = plantillaActual.Campos.FirstOrDefault(c => c.Id.Equals($"[{baseName.ToUpper()}_{filasActuales - 1}]", StringComparison.OrdinalIgnoreCase));
                if (campoPrev != null)
                {
                    var nuevoCampo = new CampoPlantilla
                    {
                        Id = $"[{baseName.ToUpper()}_{filasActuales}]",
                        Etiqueta = campoPrev.Etiqueta.Replace((filasActuales - 1).ToString(), filasActuales.ToString()),
                        Tipo = campoPrev.Tipo
                    };
                    plantillaActual.Campos.Add(nuevoCampo);
                }
            }

            plantillaActual.ContenidoBase = plantillaActual.ContenidoBase.Replace(lineaTablaOriginal, lineaTablaOriginal + "\n" + nuevaLinea);
            lineaTablaOriginal = nuevaLinea;

            GenerarCamposDinamicos();
            ActualizarVistaPrevia();
        }

        private void GenerarCamposDinamicos()
        {
            // Guardar estado previo para no borrar datos
            Dictionary<string, string> valoresPrevios = new Dictionary<string, string>();
            foreach (var kvp in controlesDinamicos)
            {
                valoresPrevios[kvp.Key] = kvp.Value.Text;
            }

            panelInputs.Controls.Clear();
            controlesDinamicos.Clear();
            if (plantillaActual == null || plantillaActual.Campos == null) return;

            Font lf = new Font("Segoe UI", 9f);
            Font tf = new Font("Segoe UI", 9.5f);

            List<string> varsTabla = AnalizarTablaExpansible();

            // Agrupar campos
            Dictionary<int, Panel> filaPaneles = new Dictionary<int, Panel>();
            FlowLayoutPanel panelGenerales = null;

            foreach (var campo in plantillaActual.Campos)
            {
                int filaAsignada = 0;
                Match m = Regex.Match(campo.Id, @"_([1-5])\]$");
                if (m.Success)
                {
                    filaAsignada = int.Parse(m.Groups[1].Value);
                }
                else if (varsTabla.Any(v => v.Equals(campo.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    filaAsignada = 1;
                }

                Control parentControl = panelInputs;

                if (filaAsignada > 0)
                {
                    if (!filaPaneles.ContainsKey(filaAsignada))
                    {
                        Panel container = new Panel { Width = 350, AutoSize = true, Margin = new Padding(0, 5, 0, 5) };
                        Button btnToggle = new Button { Text = $"Fila {filaAsignada} â–¼", Width = 350, Height = 30, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(240, 240, 245), TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
                        btnToggle.FlatAppearance.BorderSize = 0;
                        FlowLayoutPanel flp = new FlowLayoutPanel { Width = 345, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Location = new Point(5, 30), Visible = true };

                        btnToggle.Click += (s, e) => {
                            flp.Visible = !flp.Visible;
                            btnToggle.Text = flp.Visible ? $"Fila {filaAsignada} â–¼" : $"Fila {filaAsignada} â–²";
                        };

                        container.Controls.Add(btnToggle);
                        container.Controls.Add(flp);
                        panelInputs.Controls.Add(container);
                        filaPaneles.Add(filaAsignada, flp);
                    }
                    parentControl = filaPaneles[filaAsignada];
                }
                else
                {
                    if (panelGenerales == null)
                    {
                        Panel container = new Panel { Width = 350, AutoSize = true, Margin = new Padding(0, 5, 0, 5) };
                        Button btnToggle = new Button { Text = $"Campos Principales â–¼", Width = 350, Height = 30, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(240, 240, 245), TextAlign = ContentAlignment.MiddleLeft, Cursor = Cursors.Hand };
                        btnToggle.FlatAppearance.BorderSize = 0;
                        panelGenerales = new FlowLayoutPanel { Width = 345, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Location = new Point(5, 30), Visible = true };

                        btnToggle.Click += (s, e) => {
                            panelGenerales.Visible = !panelGenerales.Visible;
                            btnToggle.Text = panelGenerales.Visible ? $"Campos Principales â–¼" : $"Campos Principales â–²";
                        };

                        container.Controls.Add(btnToggle);
                        container.Controls.Add(panelGenerales);
                        panelInputs.Controls.Add(container);
                    }
                    parentControl = panelGenerales;
                }

                Label lbl = new Label { Text = campo.Etiqueta + ":", Font = lf, ForeColor = textMuted, AutoSize = true, Margin = new Padding(3, 8, 3, 2) };
                parentControl.Controls.Add(lbl);

                Control input;
                string et = campo.Etiqueta.ToLower();
                string cid = campo.Id.ToLower();
                if (campo.Tipo == "Fecha" || et.Contains("fecha") || cid.Contains("fecha"))
                    input = new DateTimePicker { Size = new Size(330, 28), Font = tf, Format = DateTimePickerFormat.Short };
                else
                {
                    TextBox tb = new TextBox { Size = new Size(330, 28), Font = tf };
                    if (campo.Tipo == "Monto" || et.Contains("cantidad") || et.Contains("precio") || et.Contains("total") || et.Contains("iva") || et.Contains("descuento") || et.Contains("monto") || cid.Contains("cantidad") || cid.Contains("precio") || cid.Contains("total") || cid.Contains("iva") || cid.Contains("descuento") || cid.Contains("monto"))
                    {
                        tb.KeyPress += (s, ev) =>
                        {
                            if (!char.IsControl(ev.KeyChar) && !char.IsDigit(ev.KeyChar) && (ev.KeyChar != '.') && (ev.KeyChar != ','))
                                ev.Handled = true;
                        };
                    }
                    input = tb;
                }
                input.Tag = campo.Id;

                // Restaurar estado
                if (valoresPrevios.ContainsKey(campo.Id))
                {
                    input.Text = valoresPrevios[campo.Id];
                }
                else if (campo.Id.EndsWith("_1]"))
                {
                    string baseKey = campo.Id.Replace("_1]", "]");
                    if (valoresPrevios.ContainsKey(baseKey))
                    {
                        input.Text = valoresPrevios[baseKey];
                    }
                }

                if (input is TextBox tbEvent) tbEvent.TextChanged += (s, ev) => ActualizarVistaPrevia();
                if (input is DateTimePicker dp) dp.ValueChanged += (s, ev) => ActualizarVistaPrevia();
                controlesDinamicos.Add(campo.Id, input);
                parentControl.Controls.Add(input);
            }

            if (varsTabla.Count > 0 && filasActuales < 5)
            {
                Button btnAgregarFila = new Button { Text = "+ Agregar Fila", Width = 330, Height = 35, Margin = new Padding(3, 15, 3, 10), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
                btnAgregarFila.Click += BtnAgregarFila_Click;
                panelInputs.Controls.Add(btnAgregarFila);
            }
        }

        private bool isCalculating = false;

        private bool IsPercentage(string keyword)
        {
            foreach (var kvp in controlesDinamicos)
            {
                string key = kvp.Key.ToUpper().Replace("[", "").Replace("]", "");
                if (key == keyword || (key.Contains(keyword) && !(keyword == "TOTAL" && key.Contains("SUBTOTAL"))))
                {
                    if (plantillaActual != null && plantillaActual.Campos != null)
                    {
                        var c = plantillaActual.Campos.FirstOrDefault(x => x.Id == kvp.Key);
                        if (c != null && c.Etiqueta.Contains("%")) return true;
                    }
                }
            }
            return false;
        }

        private decimal GetDecimalValue(params string[] keywords)
        {
            foreach (var kvp in controlesDinamicos)
            {
                string key = kvp.Key.ToUpper().Replace("[", "").Replace("]", "");
                foreach (var kw in keywords)
                {
                    if (key == kw || (key.Contains(kw) && !(kw == "TOTAL" && key.Contains("SUBTOTAL"))))
                    {
                        string text = kvp.Value.Text.Replace("$", "").Replace(",", ".");
                        if (decimal.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal val))
                            return val;
                    }
                }
            }
            return 0;
        }

        private void SetDecimalValue(string keyword, decimal value)
        {
            foreach (var kvp in controlesDinamicos)
            {
                string key = kvp.Key.ToUpper().Replace("[", "").Replace("]", "");
                if (key == keyword || (key.Contains(keyword) && !(keyword == "TOTAL" && key.Contains("SUBTOTAL"))))
                {
                    if (kvp.Value is TextBox tb)
                    {
                        string newVal = (value % 1 == 0) ? value.ToString("0") : value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
                        if (tb.Text != newVal)
                        {
                            tb.Text = newVal;
                        }
                    }
                    return;
                }
            }
        }

        private void AutocalcularMatematicas()
        {
            if (controlesDinamicos.Count == 0) return;

            decimal totalSubtotals = 0;

            // Iterar hasta 5 filas
            for (int i = 1; i <= filasActuales; i++)
            {
                string sufijo = (i == 1 && filasActuales == 1) ? "" : $"_{i}";

                decimal cant = GetDecimalValue($"CANTIDAD{sufijo}", $"CANT{sufijo}");
                decimal prec = GetDecimalValue($"PRECIO{sufijo}", $"UNITARIO{sufijo}");
                decimal subt = GetDecimalValue($"SUBTOTAL{sufijo}");

                if (cant > 0 && prec > 0)
                {
                    decimal calcSub = cant * prec;
                    if (subt != calcSub)
                    {
                        SetDecimalValue($"SUBTOTAL{sufijo}", calcSub);
                        subt = calcSub;
                    }
                }

                if (subt > 0)
                {
                    totalSubtotals += subt;
                }
            }

            // Totales globales
            decimal globalSubtotal = GetDecimalValue("SUBTOTAL");
            if (filasActuales > 1)
            {
                // Si el campo SUBTOTAL existe y no es el de una fila especifica, se setea
                if (globalSubtotal != totalSubtotals && totalSubtotals > 0)
                {
                    // No sobreescribir SUBTOTAL_1
                    if (controlesDinamicos.Keys.Any(k => k.ToUpper().Replace("[", "").Replace("]", "") == "SUBTOTAL"))
                    {
                        SetDecimalValue("SUBTOTAL", totalSubtotals);
                    }
                }
            }
            else
            {
                totalSubtotals = globalSubtotal > 0 ? globalSubtotal : totalSubtotals;
            }

            if (totalSubtotals > 0)
            {
                decimal iva = GetDecimalValue("IVA", "IMPUESTO");
                decimal descuento = GetDecimalValue("DESCUENTO", "DESC");
                decimal total = GetDecimalValue("TOTAL");

                decimal ivaMonto = iva;
                if (iva > 0 && (iva <= 100 || IsPercentage("IVA") || IsPercentage("IMPUESTO")))
                {
                    ivaMonto = totalSubtotals * (iva / 100m);
                }

                decimal descuentoMonto = descuento;
                if (descuento > 0 && (descuento <= 100 || IsPercentage("DESCUENTO") || IsPercentage("DESC")))
                {
                    descuentoMonto = totalSubtotals * (descuento / 100m);
                }

                decimal calcTotal = totalSubtotals + ivaMonto - descuentoMonto;
                if (total != calcTotal && calcTotal > 0)
                {
                    SetDecimalValue("TOTAL", calcTotal);
                }
            }
        }

        private string ObtenerTextoReemplazado()
        {
            if (plantillaActual == null) return "";

            if (!isCalculating)
            {
                isCalculating = true;
                AutocalcularMatematicas();
                isCalculating = false;
            }

            string draft = plantillaActual.ContenidoBase;
            foreach (var kvp in controlesDinamicos)
            {
                string valor = "";
                if (kvp.Value is DateTimePicker dtp)
                    valor = dtp.Value.ToString("dd/MM/yyyy");
                else
                    valor = kvp.Value.Text;
                if (string.IsNullOrWhiteSpace(valor)) valor = kvp.Key;

                string key = kvp.Key;
                if (!key.StartsWith("[")) key = "[" + key;
                if (!key.EndsWith("]")) key = key + "]";

                draft = draft.Replace(key, valor);
            }
            return draft;
        }

        private void ActualizarVistaPrevia()
        {
            string draft = ObtenerTextoReemplazado();
            if (string.IsNullOrEmpty(draft)) return;

            // Render HTML
            string html = MarkdownParser.ToHtml(draft);
            string htmlBase = $@"<html>
                <body style='font-family: Segoe UI, sans-serif; font-size: 13px; color: #333; padding: 10px; background-color: #fafafc;'>
                    {html}
                </body>
            </html>";

            webVistaPrevia.DocumentText = htmlBase;
        }

        // IA con Groq API
        private async void BtnGenerarIA_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text.Trim();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show("Escribe una descripcion del documento que necesitas.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGenerarIA.Enabled = false;
            btnGenerarIA.Text = "Generando...";
            lblEstado.Text = "Conectando con IA...";
            lblEstado.ForeColor = textMuted;

            try
            {
                PlantillaModel resultado = await groqService.GenerarPlantillaAsync(descripcion);
                rtbEditorPlantilla.Text = resultado.ContenidoBase;

                camposEditables.Clear();
                foreach (var c in resultado.Campos)
                {
                    if (!c.Id.StartsWith("[")) c.Id = "[" + c.Id;
                    if (!c.Id.EndsWith("]")) c.Id = c.Id + "]";
                    camposEditables.Add(c);
                }
                dgvCampos.DataSource = null;
                dgvCampos.DataSource = camposEditables;
                if (dgvCampos.Columns["Id"] != null) dgvCampos.Columns["Id"].ReadOnly = true;

                lblEstado.Text = "Plantilla generada por IA exitosamente";
                lblEstado.ForeColor = successGreen;
            }
            catch (Exception ex)
            {
                lblEstado.Text = "IA no disponible, usando modo local";
                lblEstado.ForeColor = Color.Orange;
                // Fallback a simulacion local
                SimularGeneracionIA(descripcion);
            }
            finally
            {
                btnGenerarIA.Enabled = true;
                btnGenerarIA.Text = "Generar plantilla con IA";
            }
        }

        private async void BtnEstilo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbEditorPlantilla.Text))
            {
                MessageBox.Show("No hay texto para mejorar. Genera una plantilla primero.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Button btn = sender as Button;
            string solicitud = btn.Tag.ToString();
            string originalText = btn.Text;

            btn.Enabled = false;
            btn.Text = "Aplicando...";

            try
            {
                string mejorado = await groqService.MejorarRedaccionAsync(rtbEditorPlantilla.Text, solicitud);
                rtbEditorPlantilla.Text = mejorado;
                DetectarCamposDesdeTexto(mejorado);
                lblEstado.Text = $"Estilo aplicado: {originalText}";
                lblEstado.ForeColor = successGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mejorar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
                btn.Text = originalText;
            }
        }

        private void SimularGeneracionIA(string descripcion)
        {
            string descLower = descripcion.ToLower();
            string contenido = "";

            if (descLower.Contains("inasistencia") || descLower.Contains("salud") || descLower.Contains("ausencia"))
                contenido = "Estimado/a [NOMBRE_JEFE]:\n\nPor medio de la presente, le informo que debido a un problema de salud no me será posible asistir a mis labores el día de hoy.\n\nMe encuentro siguiendo las indicaciones médicas para mi recuperación.\n\nAgradezco su comprensión.\n\nSaludos cordiales,\n\n[NOMBRE_EMPLEADO]";
            else if (descLower.Contains("recomendacion") || descLower.Contains("recomendaci"))
                contenido = "CARTA DE RECOMENDACIÓN\n\n[FECHA]\n\nA quien corresponda:\n\nPor medio de la presente hago constar que el/la Sr(a). [NOMBRE_EMPLEADO] laboró en [EMPRESA] desempeñando el cargo de [CARGO] con excelente desempeño.\n\nAtentamente,\n\n[NOMBRE_JEFE]\n[CARGO_JEFE]";
            else if (descLower.Contains("renuncia"))
                contenido = "CARTA DE RENUNCIA\n\n[FECHA]\n\nSeñores,\n[EMPRESA]\n\nPor la presente, comunico mi renuncia irrevocable al cargo de [CARGO].\n\nAtentamente,\n\n[NOMBRE]";
            else if (descLower.Contains("contrato") && descLower.Contains("alquiler"))
                contenido = "CONTRATO DE ARRENDAMIENTO\n\nEn [CIUDAD], a [FECHA], comparecen:\n\nARRENDADOR: [NOMBRE_ARRENDADOR]\nARRENDATARIO: [NOMBRE_ARRENDATARIO]\n\nRenta mensual: [MONTO].\nDirección: [DIRECCION].";
            else if (descLower.Contains("contrato") || descLower.Contains("laboral") || descLower.Contains("empleo"))
                contenido = "CONTRATO DE TRABAJO\n\nConste por el presente documento el contrato entre [EMPRESA] y [NOMBRE] para el cargo de [CARGO] desde [FECHA].\n\nRemuneración: [MONTO].";
            else if (descLower.Contains("vacaciones"))
                contenido = "SOLICITUD DE VACACIONES\n\n[FECHA]\n\nEstimado/a [NOMBRE_JEFE]:\n\nSolicito vacaciones del [FECHA_INICIO] al [FECHA_FIN].\n\nAtentamente,\n[NOMBRE_EMPLEADO]";
            else
                contenido = "DOCUMENTO\n\n[FECHA]\n\nEstimado/a [DESTINATARIO]:\n\n" + descripcion + "\n\nAtentamente,\n[REMITENTE]";

            rtbEditorPlantilla.Text = contenido;
            DetectarCamposDesdeTexto(contenido);
        }

        private void DetectarCamposDesdeTexto(string texto)
        {
            camposEditables.Clear();
            MatchCollection matches = Regex.Matches(texto, @"\[([^\]]+)\]");
            HashSet<string> unicos = new HashSet<string>();
            foreach (Match m in matches)
            {
                if (unicos.Add(m.Value))
                {
                    string etiqueta = m.Groups[1].Value.Replace("_", " ");
                    if (etiqueta.Length > 0)
                        etiqueta = char.ToUpper(etiqueta[0]) + etiqueta.Substring(1).ToLower();
                    string tipo = "Texto";
                    if (etiqueta.ToLower().Contains("fecha")) tipo = "Fecha";
                    if (etiqueta.ToLower().Contains("monto") || etiqueta.ToLower().Contains("precio")) tipo = "Monto";
                    camposEditables.Add(new CampoPlantilla { Id = m.Value, Etiqueta = etiqueta, Tipo = tipo });
                }
            }
            dgvCampos.DataSource = null;
            dgvCampos.DataSource = camposEditables;
            if (dgvCampos.Columns["Id"] != null) dgvCampos.Columns["Id"].ReadOnly = true;
        }

        private void BtnAgregarCampo_Click(object sender, EventArgs e)
        {
            camposEditables.Add(new CampoPlantilla { Id = "[NUEVO_CAMPO]", Etiqueta = "Nuevo campo", Tipo = "Texto" });
        }

        private void BtnEliminarCampo_Click(object sender, EventArgs e)
        {
            if (dgvCampos.SelectedRows.Count > 0)
            {
                int idx = dgvCampos.SelectedRows[0].Index;
                if (idx >= 0 && idx < camposEditables.Count)
                    camposEditables.RemoveAt(idx);
            }
        }

        private void BtnGuardarPlantilla_Click(object sender, EventArgs e)
        {
            string contenido = rtbEditorPlantilla.Text;
            if (string.IsNullOrWhiteSpace(contenido))
            {
                MessageBox.Show("No hay plantilla para guardar. Genera o edita una plantilla primero.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string titulo = "Plantilla_Generada";
            string[] lineas = contenido.Split('\n');
            if (lineas.Length > 0 && lineas[0].Trim().Length > 0 && lineas[0].Trim().Length < 80)
                titulo = lineas[0].Trim().Replace(" ", "_");

            using (var inputDialog = new Form())
            {
                inputDialog.Text = "Nombre de la plantilla";
                inputDialog.Size = new Size(400, 160);
                inputDialog.StartPosition = FormStartPosition.CenterParent;
                inputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputDialog.BackColor = Color.White;
                Label lbl = new Label { Text = "Nombre:", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 10) };
                TextBox txt = new TextBox { Text = titulo, Location = new Point(15, 45), Size = new Size(350, 30), Font = new Font("Segoe UI", 11) };
                Button ok = new Button { Text = "Guardar", DialogResult = DialogResult.OK, Location = new Point(220, 80), Size = new Size(145, 35), BackColor = successGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                ok.FlatAppearance.BorderSize = 0;
                inputDialog.Controls.AddRange(new Control[] { lbl, txt, ok });
                inputDialog.AcceptButton = ok;
                if (inputDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
                    titulo = txt.Text.Trim();
                else return;
            }

            PlantillaModel modelo = new PlantillaModel();
            modelo.Titulo = titulo;
            modelo.ContenidoBase = contenido;
            modelo.Campos = new List<CampoPlantilla>(camposEditables);
            string cleanTitle = string.Join("_", titulo.Split(Path.GetInvalidFileNameChars()));
            string jsonRuta = Path.Combine(rutaPlantillas, cleanTitle + ".json");
            File.WriteAllText(jsonRuta, JsonConvert.SerializeObject(modelo, Formatting.Indented));
            lblEstado.Text = "Plantilla guardada exitosamente";
            CargarListaPlantillas();
        }

        private void BtnCambiarCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = rutaPlantillas;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    rutaPlantillas = fbd.SelectedPath;
                    CargarListaPlantillas();
                }
            }
        }

        private void BtnNuevaPlantillaIA_Click(object sender, EventArgs e)
        {
            txtDescripcion.Focus();
            txtDescripcion.Text = "";
        }


        private void BtnExportarDocx_Click(object sender, EventArgs e)
        {
            if (webVistaPrevia.DocumentText == null || webVistaPrevia.DocumentText.Length < 20)
            {
                MessageBox.Show("Selecciona una plantilla y completa los campos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombre = "Documento";
            if (lstPlantillas.SelectedItem != null) nombre = lstPlantillas.SelectedItem.ToString();
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Documento de Word (*.docx)|*.docx", Title = "Guardar DOCX", FileName = nombre + "_Generado.docx" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(webVistaPrevia.DocumentText)))
                    using (WordDocument document = new WordDocument(ms, FormatType.Html))
                    {
                        document.Save(sfd.FileName, FormatType.Docx);
                    }
                    lblEstado.Text = "DOCX guardado exitosamente";
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar DOCX: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            string textoReemplazado = ObtenerTextoReemplazado();
            if (string.IsNullOrWhiteSpace(textoReemplazado))
            {
                MessageBox.Show("Selecciona una plantilla y completa los campos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombre = "Documento";
            if (lstPlantillas.SelectedItem != null) nombre = lstPlantillas.SelectedItem.ToString();

            string formato = cbFormatoExportar.SelectedItem.ToString();
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = $"Guardar {formato}",
                FileName = nombre + $"_Generado.{formato.ToLower()}",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (formato == "TXT") sfd.Filter = "Archivo de texto (*.txt)|*.txt";
            else if (formato == "XML") sfd.Filter = "Archivo XML (*.xml)|*.xml";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (formato == "TXT")
                    {
                        File.WriteAllText(sfd.FileName, textoReemplazado);
                    }
                    else if (formato == "XML")
                    {
                        string xml = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<DocumentoGenerado>\r\n<Plantilla>{nombre}</Plantilla>\r\n<Contenido>\r\n{System.Security.SecurityElement.Escape(textoReemplazado)}\r\n</Contenido>\r\n</DocumentoGenerado>";
                        File.WriteAllText(sfd.FileName, xml);
                    }
                    lblEstado.Text = $"{formato} guardado exitosamente";
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Cargar Documento",
                Filter = "Documentos (*.txt;*.xml)|*.txt;*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string ext = Path.GetExtension(ofd.FileName).ToLower();
                    string textoFinal = "";
                    if (ext == ".txt")
                    {
                        textoFinal = File.ReadAllText(ofd.FileName);
                    }
                    else if (ext == ".xml")
                    {
                        string xmlContent = File.ReadAllText(ofd.FileName);
                        int start = xmlContent.IndexOf("<Contenido>");
                        int end = xmlContent.IndexOf("</Contenido>");
                        if (start >= 0 && end >= 0)
                        {
                            start += 11;
                            string unescaped = xmlContent.Substring(start, end - start).Trim();
                            textoFinal = unescaped.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", "\"");
                        }
                        else
                        {
                            MessageBox.Show("Formato XML no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Render it in the WebBrowser in Read-Only Mode
                    string html = MarkdownParser.ToHtml(textoFinal);
                    string htmlBase = $@"<html>
                        <body style='font-family: Segoe UI, sans-serif; font-size: 13px; color: #333; padding: 10px; background-color: #fafafc;'>
                            <div style='background-color:#e3f2fd; padding: 8px; margin-bottom: 10px; border-radius: 4px; border-left: 4px solid #2196f3; font-size: 12px;'>
                                <strong>Modo VisualizaciÃ³n Simple:</strong> Este documento fue cargado y sus campos originales ya no son editables.
                            </div>
                            {html}
                        </body>
                    </html>";
                    webVistaPrevia.DocumentText = htmlBase;
                    lblEstado.Text = "Documento cargado";
                    lblEstado.ForeColor = primaryColor;

                    panelInputs.Controls.Clear();
                    controlesDinamicos.Clear();
                    plantillaActual = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void BtnDocGenerar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDocPrompt.Text.Trim();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show("Escribe una descripción del documento que necesitas.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnDocGenerar.Enabled = false;
            btnDocGenerar.Text = "✨ Generando...";
            flpDocInputs.Controls.Clear();
            templateRawXml = "";
            currentPlantillaModel = null;
            UpdateWebPreview("<div><center><h2 style='color:#6750A4;margin-top:50px;'>Generando plantilla con IA...</h2><p>Esto puede tardar unos segundos.</p></center></div>");

            try
            {
                PlantillaModel resultado = await groqDocService.GenerarPlantillaAsync(descripcion);
                templateRawXml = resultado.ContenidoBase;
                currentPlantillaModel = resultado;

                foreach (var c in resultado.Campos)
                {
                    string key = c.Id;
                    if (!key.StartsWith("[")) key = "[" + key;
                    if (!key.EndsWith("]")) key = key + "]";
                    AddDocInputRow(key, c.Tipo ?? "Texto", c.ValorDefecto ?? "");
                }

                AddDocInputButton();
                RenderDocPreview();
            }
            catch (Exception ex)
            {
                UpdateWebPreview($"<div><center><h2 style='color:red;margin-top:50px;'>Error</h2><p>{ex.Message}</p></center></div>");
            }
            finally
            {
                btnDocGenerar.Enabled = true;
                btnDocGenerar.Text = "✨ Generar plantilla con IA";
            }
        }

        private void AddDocInputButton()
        {
            Button btnAddInput = new Button { Text = "+ Agregar campo", Size = new Size(flpDocInputs.Width - 25, 30), BackColor = Color.White, FlatStyle = FlatStyle.Flat, ForeColor = textDark, Font = new Font("Segoe UI", 8.5f), Cursor = Cursors.Hand, Margin = new Padding(5) };
            btnAddInput.FlatAppearance.BorderColor = borderLight;
            btnAddInput.Click += BtnAddDocInput_Click;
            flpDocInputs.Controls.Add(btnAddInput);
        }

        private void BtnAddDocInput_Click(object sender, EventArgs e)
        {
            if (flpDocInputs.Controls.Count > 0 && flpDocInputs.Controls[flpDocInputs.Controls.Count - 1] is Button)
            {
                flpDocInputs.Controls.RemoveAt(flpDocInputs.Controls.Count - 1);
            }

            AddDocInputRow("[NUEVO_CAMPO]", "Texto");
            AddDocInputButton();
            RenderDocPreview();
        }

        private void AddDocInputRow(string tag, string tipo, string valorDefecto = "")
        {
            Panel row = new Panel { Size = new Size(flpDocInputs.Width - 10, 35), Margin = new Padding(2) };

            Label lblIcon = new Label { Text = "📌", AutoSize = true, Location = new Point(2, 8), Font = new Font("Segoe UI", 9f) };
            row.Controls.Add(lblIcon);

            TextBox txtTag = new TextBox { Text = tag, Location = new Point(25, 5), Size = new Size(130, 25), Font = new Font("Segoe UI", 9f) };
            txtTag.TextChanged += (s, e) => RenderDocPreview();
            row.Controls.Add(txtTag);

            TextBox txtValor = new TextBox { Text = valorDefecto ?? "", Location = new Point(160, 5), Size = new Size(130, 25), Font = new Font("Segoe UI", 9f) };
            txtValor.TextChanged += (s, e) => RenderDocPreview();
            row.Controls.Add(txtValor);

            Button btnDel = new Button { Text = "🗑️", Location = new Point(295, 4), Size = new Size(28, 28), FlatStyle = FlatStyle.Flat, ForeColor = Color.Red, BackColor = Color.White, Cursor = Cursors.Hand };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += (s, ev) => { flpDocInputs.Controls.Remove(row); RenderDocPreview(); };
            row.Controls.Add(btnDel);

            txtTag.Tag = tag;
            row.Tag = new TextBox[] { txtTag, txtValor };

            flpDocInputs.Controls.Add(row);
        }

        private void RenderDocPreview()
        {
            if (string.IsNullOrEmpty(templateRawXml) || currentPlantillaModel == null) return;

            string render = templateRawXml;

            foreach (Control c in flpDocInputs.Controls)
            {
                if (c is Panel row && row.Tag != null && row.Tag is TextBox[] textboxes)
                {
                    TextBox tTag = textboxes[0];
                    TextBox tVal = textboxes[1];

                    string clave = tTag.Text.Trim();
                    string valor = tVal.Text.Trim();

                    if (!string.IsNullOrEmpty(clave))
                    {
                        if (string.IsNullOrEmpty(valor))
                            render = render.Replace(clave, "<span class='placeholder'>" + clave + "</span>");
                        else
                            render = render.Replace(clave, "<span style='color:black;'>" + valor + "</span>");
                    }
                }
            }

            // Parseamos el XML principal a HTML visual
            string xmlHtml = CustomXmlParser.ToHtml(render);

            // Construimos el Tri-Panel combinando Header, Body y Footer
            StringBuilder htmlFull = new StringBuilder();
            htmlFull.Append("<html><body style='background-color: #E5E7EB; padding: 20px; font-family: sans-serif; display: flex; flex-direction: column; align-items: center;'>");
            htmlFull.Append("<div style='background-color: white; width: 800px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); display: flex; flex-direction: column; min-height: 1000px;'>");

            // PANEL 1: HEADER
            if (currentPlantillaModel.Header != null && currentPlantillaModel.Header.Visible)
            {
                htmlFull.Append("<div style='padding: 20px 40px; border-bottom: 2px dashed #CBD5E1; background-color: #F8FAFC; color: #64748B; font-size: 12px; display: flex; justify-content: space-between; align-items: center;'>");
                htmlFull.Append("<div><strong>[ ENCABEZADO ]</strong> <br/> Alineación: " + currentPlantillaModel.Header.Alineacion + " </div>");

                if (currentPlantillaModel.Header.MostrarLogo)
                    htmlFull.Append("<div style='padding: 10px; border: 1px solid #CBD5E1; background: white; border-radius: 4px;'>🖼️ Logo Empresarial</div>");

                if (!string.IsNullOrEmpty(currentPlantillaModel.Header.TituloSecundario))
                    htmlFull.Append("<div style='text-align: right;'>" + currentPlantillaModel.Header.TituloSecundario + "</div>");

                htmlFull.Append("</div>");
            }

            // PANEL 2: CONTENIDO BASE
            htmlFull.Append("<div style='padding: 40px; flex-grow: 1;'>");
            htmlFull.Append(xmlHtml);
            htmlFull.Append("</div>");

            // PANEL 3: FOOTER
            if (currentPlantillaModel.Footer != null && currentPlantillaModel.Footer.Visible)
            {
                htmlFull.Append("<div style='padding: 20px 40px; border-top: 2px dashed #CBD5E1; background-color: #F8FAFC; color: #64748B; font-size: 12px; display: flex; justify-content: space-between; align-items: center;'>");
                htmlFull.Append("<div><strong>[ PIE DE PÁGINA ]</strong></div>");

                if (currentPlantillaModel.Footer.MostrarFecha)
                    htmlFull.Append("<div>Fecha Impresión: " + DateTime.Now.ToString("dd/MM/yyyy") + "</div>");

                if (!string.IsNullOrEmpty(currentPlantillaModel.Footer.Texto))
                    htmlFull.Append("<div>" + currentPlantillaModel.Footer.Texto + "</div>");

                if (!string.IsNullOrEmpty(currentPlantillaModel.Footer.FormatoNumeroPagina))
                    htmlFull.Append("<div>" + currentPlantillaModel.Footer.FormatoNumeroPagina.Replace("X", "1").Replace("Y", "1") + "</div>");

                htmlFull.Append("</div>");
            }

            htmlFull.Append("</div></body></html>");

            UpdateWebPreview(htmlFull.ToString());
        }

        private void BtnDocGuardar_Click(object sender, EventArgs e)
        {
            if (currentPlantillaModel == null || string.IsNullOrEmpty(templateRawXml))
            {
                MessageBox.Show("Por favor, genera primero una plantilla de documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = currentPlantillaModel.Titulo ?? "Documento_IA";
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Documento de Word (*.docx)|*.docx", Title = "Guardar Documento DOCX", FileName = nombre.Replace(" ", "_") + ".docx" };
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Obtener y reemplazar variables
                    string render = templateRawXml;
                    foreach (Control c in flpDocInputs.Controls)
                    {
                        if (c is Panel row && row.Tag != null && row.Tag is TextBox[] textboxes)
                        {
                            string clave = textboxes[0].Text.Trim();
                            string valor = textboxes[1].Text.Trim();
                            if (!string.IsNullOrEmpty(clave))
                            {
                                render = render.Replace(clave, valor);
                            }
                        }
                    }

                    // 2. Convertir XML documental a HTML base
                    string htmlContent = CustomXmlParser.ToHtml(render);

                    // Syncfusion DOCX export
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)))
                    using (WordDocument document = new WordDocument(ms, FormatType.Html))
                    {
                        // 3. Aplicar configuración física (Márgenes, Orientación, A4)
                        if (currentPlantillaModel.Configuracion != null)
                        {
                            foreach (WSection section in document.Sections)
                            {
                                section.PageSetup.Margins.Top = (float)currentPlantillaModel.Configuracion.MargenSuperior * 28.35f; // Convertir cm a pts
                                section.PageSetup.Margins.Bottom = (float)currentPlantillaModel.Configuracion.MargenInferior * 28.35f;
                                section.PageSetup.Margins.Left = (float)currentPlantillaModel.Configuracion.MargenIzquierdo * 28.35f;
                                section.PageSetup.Margins.Right = (float)currentPlantillaModel.Configuracion.MargenDerecho * 28.35f;

                                if (currentPlantillaModel.Configuracion.Orientacion?.ToLower() == "horizontal")
                                    section.PageSetup.Orientation = PageOrientation.Landscape;
                                else
                                    section.PageSetup.Orientation = PageOrientation.Portrait;

                                if (currentPlantillaModel.Configuracion.TamanoPapel?.ToLower() == "a4")
                                    section.PageSetup.PageSize = new SizeF(595.3f, 841.9f); // A4 en puntos
                                else if (currentPlantillaModel.Configuracion.TamanoPapel?.ToLower() == "carta")
                                    section.PageSetup.PageSize = new SizeF(612f, 792f); // Carta en puntos
                            }
                        }

                        // 4. Configurar Header y Footer
                        if (currentPlantillaModel.Header != null && currentPlantillaModel.Header.Visible)
                        {
                            WSection firstSection = document.LastSection;
                            WParagraph headerParagraph = firstSection.HeadersFooters.Header.AddParagraph() as WParagraph;
                            
                            if (currentPlantillaModel.Header.Alineacion?.ToLower() == "centro")
                                headerParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                            else if (currentPlantillaModel.Header.Alineacion?.ToLower() == "derecha")
                                headerParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right;
                            else
                                headerParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
                                
                            if (!string.IsNullOrEmpty(currentPlantillaModel.Header.TituloSecundario))
                            {
                                IWTextRange textRange = headerParagraph.AppendText(currentPlantillaModel.Header.TituloSecundario);
                                textRange.CharacterFormat.TextColor = Color.Gray;
                                textRange.CharacterFormat.FontSize = 10f;
                            }
                        }

                        if (currentPlantillaModel.Footer != null && currentPlantillaModel.Footer.Visible)
                        {
                            WSection firstSection = document.LastSection;
                            WParagraph footerParagraph = firstSection.HeadersFooters.Footer.AddParagraph() as WParagraph;
                            footerParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                            if (currentPlantillaModel.Footer.MostrarFecha)
                            {
                                IWTextRange dateText = footerParagraph.AppendText($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}  -  ");
                                dateText.CharacterFormat.TextColor = Color.Gray;
                                dateText.CharacterFormat.FontSize = 10f;
                            }

                            if (!string.IsNullOrEmpty(currentPlantillaModel.Footer.Texto))
                            {
                                IWTextRange confText = footerParagraph.AppendText($"{currentPlantillaModel.Footer.Texto}  -  ");
                                confText.CharacterFormat.TextColor = Color.Gray;
                                confText.CharacterFormat.FontSize = 10f;
                            }

                            if (!string.IsNullOrEmpty(currentPlantillaModel.Footer.FormatoNumeroPagina))
                            {
                                IWTextRange pgText = footerParagraph.AppendText("Página ");
                                pgText.CharacterFormat.TextColor = Color.Gray;
                                pgText.CharacterFormat.FontSize = 10f;
                                footerParagraph.AppendField("Page", FieldType.FieldPage);
                                IWTextRange ofText = footerParagraph.AppendText(" de ");
                                ofText.CharacterFormat.TextColor = Color.Gray;
                                ofText.CharacterFormat.FontSize = 10f;
                                footerParagraph.AppendField("NumPages", FieldType.FieldNumPages);
                            }
                        }

                        // Guardar docx
                        document.Save(sfd.FileName, FormatType.Docx);
                    }

                    MessageBox.Show("Documento DOCX generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el documento DOCX: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}




