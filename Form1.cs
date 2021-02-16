using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageSeer
{
	public partial class Form1 : Form
	{
		Point p;
		int all_pm = 1;
		double origin_rate = 1.0;
		int w = 200;
		//Console.WriteLine("hello");
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
		public const int WM_SYSCOMMAND = 0x0112;

		public Form1()
		{
			InitializeComponent();
		}

		private int abs(int ab)
		{
			all_pm = (2 * ab + 1) % 2;
			return all_pm * ab;

		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			if (this.Width != w)
			{

				this.Height = (int)(this.Width / origin_rate);
			}
			else
			{
				this.Width = (int)(this.Height * origin_rate);

			}
			w = this.Width;
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			pictureBox1.Image = new Bitmap(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
			this.Width = pictureBox1.Image.Width;
			this.Height = pictureBox1.Image.Height;
			w = this.Width;
			origin_rate = pictureBox1.Image.Width / (float)pictureBox1.Image.Height;

		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else e.Effect = DragDropEffects.None;

		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			if (this.Width - abs(2 * e.X - this.Width) < 40)
			{
				this.Cursor = Cursors.SizeWE;
				SendMessage(this.Handle, WM_SYSCOMMAND, 0xF001 + (all_pm + 1) / 2 * 0x0001, 0);


			}
			else if (this.Height - abs(2 * e.Y - this.Height) < 40)
			{
				this.Cursor = Cursors.SizeNS;
				SendMessage(this.Handle, WM_SYSCOMMAND, 0xF003 + (all_pm + 1) / 2 * 0x0003, 0);
			}
			else
			{
				this.Cursor = Cursors.SizeAll;
				SendMessage(this.Handle, WM_SYSCOMMAND, 0xF012, 0);

			}

		}

	}
}
