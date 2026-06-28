// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.Principal
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ConvertidorImagenes;
using ConvertidorImagenes.Properties;

namespace ConvertidorImagenes
{
public class Principal : Form
{
	private IContainer components = null;

	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button5;

	private Label label1;

	public Principal()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Form1 form = new Form1();
		Hide();
		form.Show();
	}

	private void Principal_FormClosed(object sender, FormClosedEventArgs e)
	{
		Environment.Exit(0);
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Offices offices = new Offices();
		offices.Show();
		Hide();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		OCR oCR = new OCR();
		oCR.Show();
		Hide();
	}

	private void button5_Click(object sender, EventArgs e)
	{
		redimension redimension2 = new redimension();
		redimension2.Show();
		Hide();
	}

	private void Principal_Load(object sender, EventArgs e)
	{
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	private void label1_Click(object sender, EventArgs e)
	{
     MessageBox.Show("Convertidor de Imágenes\nVersión Alpha.");
	}

	private void button4_Click(object sender, EventArgs e)
	{
		MessageBox.Show("Pronto 7u7.");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertidorImagenes.Principal));
		this.button2 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.button2.Location = new System.Drawing.Point(266, 36);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(157, 116);
		this.button2.TabIndex = 1;
		this.button2.Text = "Office a PDF/PDF a Office- (Experimental)";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.button4.BackColor = System.Drawing.SystemColors.ActiveCaption;
		this.button4.Location = new System.Drawing.Point(266, 177);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(160, 127);
		this.button4.TabIndex = 3;
		this.button4.Text = "Ayuda";
		this.button4.UseVisualStyleBackColor = false;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.button5.Font = new System.Drawing.Font("Times New Roman", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
       this.button5.Image = null;
		this.button5.Location = new System.Drawing.Point(45, 177);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(160, 116);
		this.button5.TabIndex = 4;
		this.button5.Text = "Redimenziona y corta Imagenes";
		this.button5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.button3.Font = new System.Drawing.Font("Times New Roman", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.ForeColor = System.Drawing.SystemColors.Control;
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(469, 36);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(160, 116);
		this.button3.TabIndex = 2;
		this.button3.Text = "Reconocimiento Optico de Caracteres ";
		this.button3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button1.BackColor = System.Drawing.Color.Transparent;
		this.button1.Font = new System.Drawing.Font("Times New Roman", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
		this.button1.Location = new System.Drawing.Point(45, 36);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(160, 116);
		this.button1.TabIndex = 0;
		this.button1.Text = "Convertidor de Imagenes";
		this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label1.AutoSize = true;
		this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
		this.label1.Location = new System.Drawing.Point(554, 294);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(75, 13);
		this.label1.TabIndex = 5;
		this.label1.Text = "Version: Alpha";
		this.label1.Click += new System.EventHandler(label1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(641, 316);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button5);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.button3);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		this.ForeColor = System.Drawing.SystemColors.Highlight;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "Principal";
		this.Text = "Principal";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Principal_FormClosed);
		base.Load += new System.EventHandler(Principal_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
}
