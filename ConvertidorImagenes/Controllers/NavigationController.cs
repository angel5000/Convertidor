using System;
using System.Windows.Forms;

namespace ConvertidorImagenes.Controllers
{
	public class NavigationController
	{
		public void Show<TForm>(Form currentForm) where TForm : Form, new()
		{
			TForm form = new TForm();
			currentForm.Hide();
			form.Show();
		}

		public void CloseApplication()
		{
			Environment.Exit(0);
		}
	}
}
