using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageConvert
{
    public partial class MainForm : Form
    {
        private string[] files;

        public MainForm()
        {
            InitializeComponent();
          
        }

        private void btnSelectImages_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images (*.BMP;*.JPG;*.JPEG;*.PNG;*.WEBP)|*.BMP;*.JPG;*.PNG;*.WEBP"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = openFileDialog.FileNames;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (files != null && files.Length > 0 && lbFormats.SelectedItem != null)
            {
                lblStatus.Text = "";
                string format = lbFormats.SelectedItem.ToString();
                string newFolderPath = Path.GetDirectoryName(files[0]) + "\\yeniFormat";

                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                }

                int count = 0;
                foreach (string file in files)
                {
                    //string newFileName = Path.GetFileNameWithoutExtension(file) + "_" + txtSuffix.Text + "_" + new Random().Next(100, 9999).ToString();
                    string newFileName = txtSuffix.Text + "_" + new Random().Next(100, 9999).ToString();
                    string newFilePath = newFolderPath + "\\" + newFileName + "." + format.ToLower();

                    MagickReadSettings settings = new MagickReadSettings();
                    using (MagickImage image = new MagickImage(file, settings))
                    {
                        image.Write(newFilePath, GetMagickFormat(format));
                        count++;
                        progressBar.Value = (int)(((double)count / files.Length) * 100);
                    }
                }

                lblStatus.Text = "Dönüştürme işlemi başarılı bir şekilde tamamlandı!";
                System.Diagnostics.Process.Start("explorer.exe", newFolderPath); // New folder is opened in file explorer

            }
            else
            {
                MessageBox.Show("Lütfen dönüştürülecek dosya veya dosya formatını seçiniz!!");
            }
        }
        private MagickFormat GetMagickFormat(string format)
        {
            switch (format.ToLower())
            {
                case "jpg":
                    return MagickFormat.Jpeg;
                case "jpeg":
                    return MagickFormat.Jpeg;
                case "bmp":
                    return MagickFormat.Bmp;
                case "png":
                    return MagickFormat.Png;
                case "webp":
                    return MagickFormat.WebP;
                default:
                    throw new NotSupportedException("Belirtilen biçim desteklenmiyor!!");
            }
        }
    }
}
