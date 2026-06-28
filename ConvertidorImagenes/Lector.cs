// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.Lector
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Controls.WinForms;
using Patagames.Pdf.Net.Controls.WinForms.ToolBars;

public class Lector : Form
{
	private string Ruta = "";

	private RichTextBox txt1 = null;

	private Label lbb1 = null;

	private Label lbb2 = null;

	private Label lbff;

	private ProgressBar pbb = null;

	private Button btt1;

	private Button btc1;

	private Button btgu;

	private Button btcopi;

	private Button btlect;

	private IContainer components = null;

	private PdfViewer pdfViewer1;

	private PdfToolStripPages pdfToolStripPages1;

	private TextBox txtini;

	private TextBox textBox2;

	private Button acepag;

	private Label label1;

	private Label label2;

	private Button btcancel;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private RichTextBox richTextBox1;

	public Lector(string ruta, RichTextBox txt2, Label lb1, Label lb2, ProgressBar pb, Button bt1, Button btc, Button btg, Button btcop, Button btl, Label lbfial)
	{
		InitializeComponent();
		print(ruta);
		Ruta = ruta;
		txt1 = txt2;
		lbb1 = lb1;
		lbb2 = lb2;
		pbb = pb;
		btt1 = bt1;
		btc1 = btc;
		lbff = lbfial;
		richTextBox1.Text = "Los rangos de paginas no alteraran el contenido que se visualiza por lo que no hara ningun cambion si escribe un rango de paginas pero si afectara al momento de hacer la letura, esto por motivos de falta de recursos.";
		btgu = btg;
		btcopi = btcop;
		btlect = btl;
	}

	private void Lector_Load(object sender, EventArgs e)
	{
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	public void print(string ruta)
	{
		pdfViewer1.LoadDocument(ruta);
	}

	private void pdfToolStripPages1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
	}

	public void render(RichTextBox txt)
	{
		btc1.Enabled = false;
		btlect.Enabled = false;
		int num = 1;
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using (PdfReader pdfReader = new PdfReader(Ruta))
			{
				pdfReader.SelectPages(txtini.Text + "-" + textBox2.Text);
				int.TryParse(txtini.Text, out var result);
				for (num = result; num <= pdfReader.NumberOfPages; num++)
				{
					string textFromPage = PdfTextExtractor.GetTextFromPage(pdfReader, num, new LocationTextExtractionStrategy());
					stringBuilder.Append(textFromPage);
					pbb.Value = 100;
					lbb2.Text = "Finalizo!";
					lbb1.Text = "Finalizado!";
					lbff.Text = "100";
				}
				pdfReader.Close();
			}
			btt1.Enabled = true;
			btcopi.Enabled = true;
			btgu.Enabled = true;
			txt.Text = stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void acepag_Click(object sender, EventArgs e)
	{
		bool flag = false;
		bool flag2 = false;
		int.TryParse(textBox2.Text, out var result);
		if (int.TryParse(txtini.Text, out var result2) && int.TryParse(textBox2.Text, out var result3) && result2 == result3 && !flag2)
		{
			textBox2.Text = "";
			txtini.Text = "";
			flag = false;
		}
		if (int.TryParse(txtini.Text, out var result4) && int.TryParse(textBox2.Text, out var result5) && result5 < result4 && !flag2)
		{
			textBox2.Text = "";
			txtini.Text = "";
			flag = false;
		}
		if (result > pdfViewer1.Document.Pages.Count)
		{
			flag = false;
		}
		else
		{
			flag2 = true;
		}
		if (int.TryParse(txtini.Text, out var result6) && int.TryParse(textBox2.Text, out var result7) && result6 < result7 && flag2)
		{
			flag = true;
		}
		if (!flag)
		{
			MessageBox.Show("Ingrese un rango valido");
		}
		if (flag && flag2)
		{
			render(txt1);
			Hide();
		}
	}

	private void txtini_KeyPress(object sender, KeyPressEventArgs e)
	{
		int.TryParse(txtini.Text, out var result);
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
		if (result < 1 || result >= pdfViewer1.Document.Pages.Count)
		{
			txtini.Text = "";
		}
	}

	private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
	{
		int.TryParse(textBox2.Text, out var result);
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
		if (result < 1 || result >= pdfViewer1.Document.Pages.Count)
		{
			textBox2.Text = "";
		}
	}

	private void txtini_TextChanged(object sender, EventArgs e)
	{
	}

	private void richTextBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void btcancel_Click(object sender, EventArgs e)
	{
		Hide();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

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
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		base.SuspendLayout();
		this.pdfViewer1.BackColor = System.Drawing.SystemColors.ControlDark;
		this.pdfViewer1.CurrentIndex = -1;
		this.pdfViewer1.CurrentPageHighlightColor = System.Drawing.Color.FromArgb(170, 70, 130, 180);
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
		this.pdfViewer1.RenderFlags = Patagames.Pdf.Enums.RenderFlags.FPDF_LCD_TEXT | Patagames.Pdf.Enums.RenderFlags.FPDF_NO_CATCH;
		this.pdfViewer1.ShowCurrentPageHighlight = true;
		this.pdfViewer1.ShowLoadingIcon = true;
		this.pdfViewer1.ShowPageSeparator = true;
		this.pdfViewer1.Size = new System.Drawing.Size(490, 542);
		this.pdfViewer1.SizeMode = Patagames.Pdf.Net.Controls.WinForms.SizeModes.FitToWidth;
		this.pdfViewer1.TabIndex = 0;
		this.pdfViewer1.TextSelectColor = System.Drawing.Color.FromArgb(70, 70, 130, 180);
		this.pdfViewer1.TilesCount = 2;
		this.pdfViewer1.UseProgressiveRender = true;
		this.pdfViewer1.ViewMode = Patagames.Pdf.Net.Controls.WinForms.ViewModes.Vertical;
		this.pdfViewer1.Zoom = 1f;
		this.pdfToolStripPages1.Location = new System.Drawing.Point(0, 0);
		this.pdfToolStripPages1.Name = "pdfToolStripPages1";
		this.pdfToolStripPages1.PdfViewer = this.pdfViewer1;
		this.pdfToolStripPages1.Size = new System.Drawing.Size(773, 27);
		this.pdfToolStripPages1.TabIndex = 1;
		this.pdfToolStripPages1.Text = "pdfToolStripPages1";
		this.pdfToolStripPages1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(pdfToolStripPages1_ItemClicked);
		this.txtini.Location = new System.Drawing.Point(632, 145);
		this.txtini.Name = "txtini";
		this.txtini.Size = new System.Drawing.Size(30, 20);
		this.txtini.TabIndex = 2;
		this.txtini.TextChanged += new System.EventHandler(txtini_TextChanged);
		this.txtini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtini_KeyPress);
		this.textBox2.Location = new System.Drawing.Point(688, 145);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(30, 20);
		this.textBox2.TabIndex = 3;
		this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox2_KeyPress);
		this.acepag.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.acepag.Location = new System.Drawing.Point(531, 190);
		this.acepag.Name = "acepag";
		this.acepag.Size = new System.Drawing.Size(82, 32);
		this.acepag.TabIndex = 4;
		this.acepag.Text = "Aceptar";
		this.acepag.UseVisualStyleBackColor = true;
		this.acepag.Click += new System.EventHandler(acepag_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(668, 143);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 20);
		this.label1.TabIndex = 5;
		this.label1.Text = "-";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(528, 148);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 13);
		this.label2.TabIndex = 6;
		this.label2.Text = "Rango de Paginas:";
		this.btcancel.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btcancel.Location = new System.Drawing.Point(641, 190);
		this.btcancel.Name = "btcancel";
		this.btcancel.Size = new System.Drawing.Size(82, 32);
		this.btcancel.TabIndex = 7;
		this.btcancel.Text = "Cancelar";
		this.btcancel.UseVisualStyleBackColor = true;
		this.btcancel.Click += new System.EventHandler(btcancel_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(594, 129);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 13);
		this.label3.TabIndex = 8;
		this.label3.Text = "Pagina Inicio";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(685, 129);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 13);
		this.label4.TabIndex = 9;
		this.label4.Text = "Pagina Final";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(524, 92);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(102, 13);
		this.label5.TabIndex = 10;
		this.label5.Text = "Ejemplo de Rangos:";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(629, 92);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(94, 13);
		this.label6.TabIndex = 11;
		this.label6.Text = "1-3; 5-6;10-15, etc";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Times New Roman", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(524, 45);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(160, 23);
		this.label7.TabIndex = 12;
		this.label7.Text = "Visualizador PDF";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Times New Roman", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(528, 242);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(44, 17);
		this.label8.TabIndex = 13;
		this.label8.Text = "Aviso:";
		this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.richTextBox1.Enabled = false;
		this.richTextBox1.Location = new System.Drawing.Point(531, 273);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.ReadOnly = true;
		this.richTextBox1.Size = new System.Drawing.Size(219, 124);
		this.richTextBox1.TabIndex = 15;
		this.richTextBox1.Text = "";
		this.richTextBox1.TextChanged += new System.EventHandler(richTextBox1_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(773, 599);
		base.ControlBox = false;
		base.Controls.Add(this.richTextBox1);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.btcancel);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.acepag);
		base.Controls.Add(this.textBox2);
		base.Controls.Add(this.txtini);
		base.Controls.Add(this.pdfToolStripPages1);
		base.Controls.Add(this.pdfViewer1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Name = "Lector";
		this.Text = "Lector";
		base.Load += new System.EventHandler(Lector_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
