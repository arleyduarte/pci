using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PDFSender.com.amdp.utils;
using System.IO;

namespace ResizeImage
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resizeImages();
            Environment.Exit(0);
        }

        private void resizeImages()
        {
            FileManager fileManager = new FileManager();
            foreach(String filePath in fileManager.getFilesOfInputDirectory(textBox1.Text, "*.jpg"))
            {
                if(System.IO.File.Exists(filePath))
                {
                    

                    String fileName = Path.GetFileName(filePath);
                   
                    FileInfo f = new FileInfo(filePath);
                    long s1 = f.Length;

                    double sizeInKilos = Convert.ToDouble(s1) / Convert.ToDouble(1024);

                    int imageSize = (int)sizeInKilos;

                    if (imageSize >= Convert.ToInt32(textBox3.Text))
                    {
                        Image image = Image.FromFile(filePath);

                        ImageUtil imageUtil = new ImageUtil();
                        imageUtil.compress(image, textBox2.Text + "\\" + fileName);
                    }
                    

                }
            }
        }
    }
}
