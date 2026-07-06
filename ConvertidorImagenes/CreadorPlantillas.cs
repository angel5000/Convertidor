using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace ConvertidorImagenes
{
    public class CreadorPlantillas : Form
    {
        private RichTextBox rtbContenido;
        private DataGridView dgvCampos;
        private TextBox txtTitulo;
        private Label lblEstado;
        private string rutaCarpetas;
        private BindingSource bindingSource;
        private System.ComponentModel.BindingList<CampoPlantilla> camposDetectados;

        public CreadorPlantillas(string rutaDefecto)
        {
            this.rutaCarpetas = rutaDefecto;
            camposDetectados = new System.ComponentModel.BindingList<CampoPlantilla>();
            bindingSource = new BindingSource();
            
            this.DoubleBuffered = true;
            ConstruirUI();
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
            this.MouseDown += new MouseEventHandler(DragForm_MouseDown);
            this.ClientSize = new Size(1100, 720);
            this.BackColor = Color.FromArgb(245, 246, 248);
            this.Text = "Creador de Plantillas";
            this.FormBorderStyle = FormBorderStyle.Sizable;

            Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
            Font subtitleFont = new Font("Segoe UI", 12, FontStyle.Bold);
            Font regularFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            
            Color primaryColor = Color.FromArgb(103, 80, 164);
            Color secondaryColor = Color.White;

            Label titleLabel = new Label { Text = "Creador de Plantillas JSON", Font = titleFont, Location = new Point(40, 15), AutoSize = true };
            this.Controls.Add(titleLabel);

            Label lblInstrucciones = new Label { Text = "Escribe tu plantilla. Usa corchetes para las variables: [Nombre]. En la tabla puedes seleccionar y presionar Supr para borrar campos.", Font = regularFont, ForeColor = Color.Gray, Location = new Point(40, 45), AutoSize = true };
            this.Controls.Add(lblInstrucciones);

            // Left Side: Editor
            RoundedPanel pnlEditor = new RoundedPanel { BackColor = Color.White, Location = new Point(40, 80), Size = new Size(600, 600), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(pnlEditor);

            Label lblTituloTemplate = new Label { Text = "Título de la Plantilla:", Font = regularFont, Location = new Point(20, 20), AutoSize = true };
            pnlEditor.Controls.Add(lblTituloTemplate);

            txtTitulo = new TextBox { Location = new Point(20, 45), Size = new Size(560, 25), Font = subtitleFont };
            pnlEditor.Controls.Add(txtTitulo);

            rtbContenido = new RichTextBox { Location = new Point(20, 80), Size = new Size(560, 500), Font = new Font("Segoe UI", 10.5f), BorderStyle = BorderStyle.FixedSingle };
            pnlEditor.Controls.Add(rtbContenido);

            // Right Side: Detect Variables
            RoundedPanel pnlVariables = new RoundedPanel { BackColor = Color.White, Location = new Point(660, 80), Size = new Size(400, 600), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(pnlVariables);

            Button btnDetectar = new Button { Text = "Detectar Variables", Location = new Point(20, 20), Size = new Size(360, 40), BackColor = secondaryColor, ForeColor = Color.Black, FlatStyle = FlatStyle.Flat, Font = regularFont, Cursor = Cursors.Hand };
            btnDetectar.Click += BtnDetectar_Click;
            pnlVariables.Controls.Add(btnDetectar);

            dgvCampos = new DataGridView { Location = new Point(20, 80), Size = new Size(360, 430), AllowUserToAddRows = false, AllowUserToDeleteRows = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, Font = regularFont };
            pnlVariables.Controls.Add(dgvCampos);

            Button btnGuardar = new Button { Text = "Guardar Plantilla", Location = new Point(20, 530), Size = new Size(360, 50), BackColor = primaryColor, ForeColor = secondaryColor, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;
            pnlVariables.Controls.Add(btnGuardar);

            lblEstado = new Label { Text = "", Font = regularFont, ForeColor = Color.Green, Location = new Point(20, 585), AutoSize = true };
            pnlVariables.Controls.Add(lblEstado);
        }

        private void BtnDetectar_Click(object sender, EventArgs e)
        {
            camposDetectados.Clear();
            string texto = rtbContenido.Text;
            
            // Buscar [TEXTO_MAYUSCULA]
            MatchCollection matches = Regex.Matches(texto, @"\[([^\]]+)\]");
            
            HashSet<string> unicos = new HashSet<string>();
            foreach(Match m in matches)
            {
                unicos.Add(m.Value);
            }

            foreach(string id in unicos)
            {
                camposDetectados.Add(new CampoPlantilla { Id = id, Etiqueta = id.Replace("[", "").Replace("]", "").Replace("_", " "), Tipo = "Texto" });
            }

            bindingSource.DataSource = null;
            bindingSource.DataSource = camposDetectados;
            dgvCampos.DataSource = bindingSource;

            // Make 'Id' readonly
            if(dgvCampos.Columns["Id"] != null) dgvCampos.Columns["Id"].ReadOnly = true;
            
            // Convert 'Tipo' column to ComboBox if possible, or leave as text
            if(dgvCampos.Columns["Tipo"] != null)
            {
                DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
                comboCol.Name = "TipoCombo";
                comboCol.HeaderText = "Tipo";
                comboCol.DataPropertyName = "Tipo";
                comboCol.Items.Add("Texto");
                comboCol.Items.Add("Fecha");
                comboCol.Items.Add("Monto");
                
                int index = dgvCampos.Columns["Tipo"].Index;
                dgvCampos.Columns.Remove("Tipo");
                dgvCampos.Columns.Insert(index, comboCol);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(rtbContenido.Text))
            {
                MessageBox.Show("El título y el contenido son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PlantillaModel modelo = new PlantillaModel();
            modelo.Titulo = txtTitulo.Text;
            modelo.ContenidoBase = rtbContenido.Text;
            modelo.Campos = new List<CampoPlantilla>();

            if (bindingSource.DataSource != null)
            {
                foreach(var item in (System.ComponentModel.BindingList<CampoPlantilla>)bindingSource.DataSource)
                {
                    modelo.Campos.Add(new CampoPlantilla { Id = item.Id, Etiqueta = item.Etiqueta, Tipo = item.Tipo });
                }
            }

            if (!Directory.Exists(rutaCarpetas))
            {
                Directory.CreateDirectory(rutaCarpetas);
            }

            string cleanTitle = string.Join("_", txtTitulo.Text.Split(Path.GetInvalidFileNameChars()));
            string jsonRuta = Path.Combine(rutaCarpetas, cleanTitle + ".json");

            string jsonString = JsonConvert.SerializeObject(modelo, Formatting.Indented);
            File.WriteAllText(jsonRuta, jsonString);

            lblEstado.Text = "Guardado exitoso!";
            MessageBox.Show("Plantilla guardada en: " + jsonRuta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}


