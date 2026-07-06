// ConvertidorImagenes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ConvertidorImagenes.visualizador
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class visualizador : Form
{
	private int px = 0;

	private int py = 0;

	private int a = 0;

	private int b = 0;

	private int c = 0;

	private int d = 0;

	private int a1 = 0;

	private int b2 = 0;

	private int c3 = 0;

	private int d4 = 0;

	private int contador = 1;

	private IContainer components = null;

	private PictureBox visual;

	private Panel panel1;

	private Button btzomout;

	private Button btzomin;

	private Label label1;

	private void visual_Click(object sender, EventArgs e)
	{
	}

	private void visualizador_Load(object sender, EventArgs e)
	{
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point((Screen.PrimaryScreen.Bounds.Width - base.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2);
	}

	public visualizador(Image img)
	{
		InitializeComponent();
		imagen(img);
		btzomout.Enabled = false;
        ApplyModernStyles();
	}

	public void imagen(Image img1)
	{
		visual.Image = img1;
		int num = visual.Image.Width;
		int num2 = visual.Image.Height;
		float num3 = Math.Min((float)visual.Width / (float)num, (float)visual.Height / (float)num2);
		num = (int)((float)num * num3);
		num2 = (int)((float)num2 * num3);
		visual.ClientSize = new Size(num, num2);
		int num4 = (panel1.Width - visual.Width) / 2;
		int num5 = (panel1.Height - visual.Height) / 2;
		visual.Location = new Point(num4, num5);
		px = visual.Location.X;
		py = visual.Location.Y;
		a = (int)((double)visual.Top - (double)visual.Height * 0.075);
		b = (int)((double)visual.Left - (double)visual.Width * 0.075);
		c = (int)((double)visual.Height + (double)visual.Height * 0.2);
		d = (int)((double)visual.Width + (double)visual.Width * 0.2);
		a1 = a;
		b2 = b;
		c3 = c;
		d4 = d;
	}

	private void btzomin_Click(object sender, EventArgs e)
	{
		if (contador <= 5)
		{
			visual.Top = (int)((double)visual.Top - (double)visual.Height * 0.075);
			visual.Left = (int)((double)visual.Left - (double)visual.Width * 0.075);
			visual.Height = (int)((double)visual.Height + (double)visual.Height * 0.2);
			visual.Width = (int)((double)visual.Width + (double)visual.Width * 0.2);
			contador++;
			btzomout.Enabled = true;
		}
	}

	private void btzomout_Click(object sender, EventArgs e)
	{
		if (contador >= 2)
		{
			visual.Top = (int)((double)visual.Top + (double)visual.Height * 0.075);
			visual.Left = (int)((double)visual.Left + (double)visual.Width * 0.075);
			visual.Height = (int)((double)visual.Height - (double)visual.Height * 0.2);
			visual.Width = (int)((double)visual.Width - (double)visual.Width * 0.2);
			contador--;
		}
		if (contador <= 2)
		{
			contador = 1;
			btzomout.Enabled = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

    private TrackBar trZoom;
    private Label lbZoom;

    private void ApplyModernStyles()
    {
        this.BackColor = Color.FromArgb(245, 246, 248);
        
        if (btzomin != null) btzomin.Visible = false;
        if (btzomout != null) btzomout.Visible = false;

        trZoom = new TrackBar { Minimum = 10, Maximum = 500, Value = 100, TickStyle = TickStyle.None, Location = new Point(190, 510), Size = new Size(200, 45) };
        trZoom.Scroll += (s, e) => {
            lbZoom.Text = trZoom.Value + "%";
            float scale = trZoom.Value / 100f;
            if (visual.Image != null)
            {
                visual.Width = (int)(visual.Image.Width * scale);
                visual.Height = (int)(visual.Image.Height * scale);
                visual.Left = (panel1.Width - visual.Width) / 2;
                visual.Top = (panel1.Height - visual.Height) / 2;
            }
        };

        lbZoom = new Label { Text = "100%", Location = new Point(390, 515), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold) };

        this.Controls.Add(trZoom);
        this.Controls.Add(lbZoom);

        if (label1 != null) { label1.Font = new Font("Segoe UI", 14f, FontStyle.Bold); label1.ForeColor = Color.FromArgb(103, 80, 164); }
    }

	private void InitializeComponent()
	{
		this.panel1 = new System.Windows.Forms.Panel();
		this.visual = new System.Windows.Forms.PictureBox();
		this.btzomout = new System.Windows.Forms.Button();
		this.btzomin = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.visual).BeginInit();
		base.SuspendLayout();
		this.panel1.AutoSize = true;
		this.panel1.Controls.Add(this.visual);
		this.panel1.Location = new System.Drawing.Point(12, 39);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(554, 466);
		this.panel1.TabIndex = 1;
		this.visual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.visual.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.visual.Location = new System.Drawing.Point(29, 29);
		this.visual.Name = "visual";
		this.visual.Size = new System.Drawing.Size(497, 425);
		this.visual.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.visual.TabIndex = 0;
		this.visual.TabStop = false;
		this.visual.Click += new System.EventHandler(visual_Click);
		this.btzomout.Location = new System.Drawing.Point(277, 511);
		this.btzomout.Name = "btzomout";
		this.btzomout.Size = new System.Drawing.Size(50, 23);
		this.btzomout.TabIndex = 18;
		this.btzomout.Text = "Zoom-";
		this.btzomout.UseVisualStyleBackColor = true;
		this.btzomout.Click += new System.EventHandler(btzomout_Click);
		this.btzomin.Location = new System.Drawing.Point(198, 511);
		this.btzomin.Name = "btzomin";
		this.btzomin.Size = new System.Drawing.Size(50, 23);
		this.btzomin.TabIndex = 17;
		this.btzomin.Text = "Zoom+";
		this.btzomin.UseVisualStyleBackColor = true;
		this.btzomin.Click += new System.EventHandler(btzomin_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(236, 27);
		this.label1.TabIndex = 19;
		this.label1.Text = "Visualizador de Imagen";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(574, 542);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btzomout);
		base.Controls.Add(this.btzomin);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "visualizador";
		this.Text = "visualizador";
		base.Load += new System.EventHandler(visualizador_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.visual).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
