
namespace ConvertidorImagenes
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btruta = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btremove = new System.Windows.Forms.Button();
            this.lbformat = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbsms2 = new System.Windows.Forms.Label();
            this.lbsms = new System.Windows.Forms.Label();
            this.lbprogre = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.progreso = new System.Windows.Forms.ProgressBar();
            this.lbft = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbformatos = new System.Windows.Forms.ComboBox();
            this.lbrutadest = new System.Windows.Forms.Label();
            this.lbruta = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnomb = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(101, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 263);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragDrop);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragEnter);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Agrega o Arrastra una imagen";
            // 
            // btruta
            // 
            this.btruta.Location = new System.Drawing.Point(6, 327);
            this.btruta.Name = "btruta";
            this.btruta.Size = new System.Drawing.Size(108, 23);
            this.btruta.TabIndex = 2;
            this.btruta.Text = "Seleccionar Ruta";
            this.btruta.UseVisualStyleBackColor = true;
            this.btruta.Click += new System.EventHandler(this.btruta_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btremove);
            this.panel1.Controls.Add(this.lbformat);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.lbsms2);
            this.panel1.Controls.Add(this.lbsms);
            this.panel1.Controls.Add(this.lbprogre);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.progreso);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lbft);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbformatos);
            this.panel1.Controls.Add(this.lbrutadest);
            this.panel1.Controls.Add(this.lbruta);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btruta);
            this.panel1.Location = new System.Drawing.Point(18, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 360);
            this.panel1.TabIndex = 4;
            // 
            // btremove
            // 
            this.btremove.Location = new System.Drawing.Point(510, 166);
            this.btremove.Name = "btremove";
            this.btremove.Size = new System.Drawing.Size(108, 23);
            this.btremove.TabIndex = 6;
            this.btremove.Text = "Otra conversion";
            this.btremove.UseVisualStyleBackColor = true;
            this.btremove.Click += new System.EventHandler(this.btremove_Click);
            // 
            // lbformat
            // 
            this.lbformat.AutoSize = true;
            this.lbformat.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbformat.Location = new System.Drawing.Point(630, 133);
            this.lbformat.Name = "lbformat";
            this.lbformat.Size = new System.Drawing.Size(24, 13);
            this.lbformat.TabIndex = 16;
            this.lbformat.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label9.Location = new System.Drawing.Point(510, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Formato seleccionado:";
            // 
            // lbsms2
            // 
            this.lbsms2.AutoSize = true;
            this.lbsms2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbsms2.Location = new System.Drawing.Point(228, 307);
            this.lbsms2.Name = "lbsms2";
            this.lbsms2.Size = new System.Drawing.Size(152, 13);
            this.lbsms2.TabIndex = 14;
            this.lbsms2.Text = "(Ruta de la imagen a convertir)";
            // 
            // lbsms
            // 
            this.lbsms.AutoSize = true;
            this.lbsms.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbsms.Location = new System.Drawing.Point(346, 332);
            this.lbsms.Name = "lbsms";
            this.lbsms.Size = new System.Drawing.Size(273, 13);
            this.lbsms.TabIndex = 13;
            this.lbsms.Text = "(Los archivos se guardaran en esta ruta predeterminada)";
            // 
            // lbprogre
            // 
            this.lbprogre.AutoSize = true;
            this.lbprogre.BackColor = System.Drawing.Color.Transparent;
            this.lbprogre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbprogre.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbprogre.Location = new System.Drawing.Point(595, 275);
            this.lbprogre.Name = "lbprogre";
            this.lbprogre.Size = new System.Drawing.Size(24, 13);
            this.lbprogre.TabIndex = 12;
            this.lbprogre.Text = "";
            this.lbprogre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label8.Location = new System.Drawing.Point(524, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Progreso";
            // 
            // progreso
            // 
            this.progreso.Location = new System.Drawing.Point(527, 270);
            this.progreso.Name = "progreso";
            this.progreso.Size = new System.Drawing.Size(175, 23);
            this.progreso.TabIndex = 10;
            // 
            // lbft
            // 
            this.lbft.AutoSize = true;
            this.lbft.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbft.Location = new System.Drawing.Point(615, 41);
            this.lbft.Name = "lbft";
            this.lbft.Size = new System.Drawing.Size(24, 13);
            this.lbft.TabIndex = 9;
            this.lbft.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label6.Location = new System.Drawing.Point(510, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Formato detectado:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label5.Location = new System.Drawing.Point(510, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Seleccione formato a convertir";
            // 
            // cbformatos
            // 
            this.cbformatos.FormattingEnabled = true;
            this.cbformatos.Location = new System.Drawing.Point(513, 98);
            this.cbformatos.Name = "cbformatos";
            this.cbformatos.Size = new System.Drawing.Size(153, 21);
            this.cbformatos.TabIndex = 6;
            this.cbformatos.Text = "Formatos...";
            this.cbformatos.SelectedIndexChanged += new System.EventHandler(this.cbformatos_SelectedIndexChanged);
            // 
            // lbrutadest
            // 
            this.lbrutadest.AutoSize = true;
            this.lbrutadest.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbrutadest.Location = new System.Drawing.Point(198, 332);
            this.lbrutadest.Name = "lbrutadest";
            this.lbrutadest.Size = new System.Drawing.Size(24, 13);
            this.lbrutadest.TabIndex = 5;
            this.lbrutadest.Text = "";
            // 
            // lbruta
            // 
            this.lbruta.AutoSize = true;
            this.lbruta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbruta.Location = new System.Drawing.Point(198, 306);
            this.lbruta.Name = "lbruta";
            this.lbruta.Size = new System.Drawing.Size(24, 13);
            this.lbruta.TabIndex = 4;
            this.lbruta.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label4.Location = new System.Drawing.Point(125, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ruta Destino:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label3.Location = new System.Drawing.Point(125, 306);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ruta Origen:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Convertidor de Imagenes";
            // 
            // btnomb
            // 
            this.btnomb.Location = new System.Drawing.Point(359, 18);
            this.btnomb.Name = "btnomb";
            this.btnomb.Size = new System.Drawing.Size(108, 23);
            this.btnomb.TabIndex = 5;
            this.btnomb.Text = "Aceptar";
            this.btnomb.UseVisualStyleBackColor = true;
            this.btnomb.Click += new System.EventHandler(this.btconver_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.txtnombre);
            this.panel2.Controls.Add(this.btnomb);
            this.panel2.Location = new System.Drawing.Point(18, 444);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 68);
            this.panel2.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(141, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Cambiar nombre Archivo";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtnombre
            // 
            this.txtnombre.Location = new System.Drawing.Point(179, 18);
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.Size = new System.Drawing.Size(159, 20);
            this.txtnombre.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(702, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Principal";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 630);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Convertidor de Imagenes";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btruta;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbrutadest;
        private System.Windows.Forms.Label lbruta;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnomb;
        private System.Windows.Forms.Button btremove;
        private System.Windows.Forms.Label lbft;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbformatos;
        private System.Windows.Forms.ProgressBar progreso;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbprogre;
        private System.Windows.Forms.Label lbsms;
        private System.Windows.Forms.Label lbsms2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbformat;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}


