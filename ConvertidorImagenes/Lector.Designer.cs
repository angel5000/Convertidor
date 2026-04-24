
namespace ConvertidorImagenes
{
    partial class Lector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pdfViewer1 = new Patagames.Pdf.Net.Controls.WinForms.PdfViewer();
            this.pdfToolStripPages1 = new Patagames.Pdf.Net.Controls.WinForms.ToolBars.PdfToolStripPages();
            this.txtini = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.acepag = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btcancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pdfViewer1.CurrentIndex = -1;
            this.pdfViewer1.CurrentPageHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.pdfViewer1.Document = null;
            this.pdfViewer1.FormHighlightColor = System.Drawing.Color.Transparent;
            this.pdfViewer1.FormsBlendMode = Patagames.Pdf.Enums.BlendTypes.FXDIB_BLEND_MULTIPLY;
            this.pdfViewer1.LoadingIconText = "Loading...";
            this.pdfViewer1.Location = new System.Drawing.Point(32, 45);
            this.pdfViewer1.MouseMode = Patagames.Pdf.Net.Controls.WinForms.MouseModes.Default;
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.OptimizedLoadThreshold = 1000;
            this.pdfViewer1.Padding = new System.Windows.Forms.Padding(10);
            this.pdfViewer1.PageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pdfViewer1.PageAutoDispose = true;
            this.pdfViewer1.PageBackColor = System.Drawing.Color.White;
            this.pdfViewer1.PageBorderColor = System.Drawing.Color.Black;
            this.pdfViewer1.PageMargin = new System.Windows.Forms.Padding(10);
            this.pdfViewer1.PageSeparatorColor = System.Drawing.Color.Gray;
            this.pdfViewer1.RenderFlags = ((Patagames.Pdf.Enums.RenderFlags)((Patagames.Pdf.Enums.RenderFlags.FPDF_LCD_TEXT | Patagames.Pdf.Enums.RenderFlags.FPDF_NO_CATCH)));
            this.pdfViewer1.ShowCurrentPageHighlight = true;
            this.pdfViewer1.ShowLoadingIcon = true;
            this.pdfViewer1.ShowPageSeparator = true;
            this.pdfViewer1.Size = new System.Drawing.Size(490, 542);
            this.pdfViewer1.SizeMode = Patagames.Pdf.Net.Controls.WinForms.SizeModes.FitToWidth;
            this.pdfViewer1.TabIndex = 0;
            this.pdfViewer1.TextSelectColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.pdfViewer1.TilesCount = 2;
            this.pdfViewer1.UseProgressiveRender = true;
            this.pdfViewer1.ViewMode = Patagames.Pdf.Net.Controls.WinForms.ViewModes.Vertical;
            this.pdfViewer1.Zoom = 1F;
            // 
            // pdfToolStripPages1
            // 
            this.pdfToolStripPages1.Location = new System.Drawing.Point(0, 0);
            this.pdfToolStripPages1.Name = "pdfToolStripPages1";
            this.pdfToolStripPages1.PdfViewer = this.pdfViewer1;
            this.pdfToolStripPages1.Size = new System.Drawing.Size(773, 27);
            this.pdfToolStripPages1.TabIndex = 1;
            this.pdfToolStripPages1.Text = "pdfToolStripPages1";
            this.pdfToolStripPages1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.pdfToolStripPages1_ItemClicked);
            // 
            // txtini
            // 
            this.txtini.Location = new System.Drawing.Point(632, 145);
            this.txtini.Name = "txtini";
            this.txtini.Size = new System.Drawing.Size(30, 20);
            this.txtini.TabIndex = 2;
            this.txtini.TextChanged += new System.EventHandler(this.txtini_TextChanged);
            this.txtini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtini_KeyPress);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(688, 145);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(30, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // acepag
            // 
            this.acepag.Location = new System.Drawing.Point(531, 190);
            this.acepag.Name = "acepag";
            this.acepag.Size = new System.Drawing.Size(75, 23);
            this.acepag.TabIndex = 4;
            this.acepag.Text = "Aceptar";
            this.acepag.UseVisualStyleBackColor = true;
            this.acepag.Click += new System.EventHandler(this.acepag_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(668, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(528, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Rango de Paginas:";
            // 
            // btcancel
            // 
            this.btcancel.Location = new System.Drawing.Point(632, 190);
            this.btcancel.Name = "btcancel";
            this.btcancel.Size = new System.Drawing.Size(75, 23);
            this.btcancel.TabIndex = 7;
            this.btcancel.Text = "Cancelar";
            this.btcancel.UseVisualStyleBackColor = true;
            this.btcancel.Click += new System.EventHandler(this.btcancel_Click);
            // 
            // Lector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 599);
            this.ControlBox = false;
            this.Controls.Add(this.btcancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acepag);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtini);
            this.Controls.Add(this.pdfToolStripPages1);
            this.Controls.Add(this.pdfViewer1);
            this.Name = "Lector";
            this.Text = "Lector";
            this.Load += new System.EventHandler(this.Lector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Patagames.Pdf.Net.Controls.WinForms.PdfViewer pdfViewer1;
        private Patagames.Pdf.Net.Controls.WinForms.ToolBars.PdfToolStripPages pdfToolStripPages1;
        private System.Windows.Forms.TextBox txtini;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button acepag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btcancel;
    }
}