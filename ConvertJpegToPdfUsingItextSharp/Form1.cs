using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.IO;
namespace ConvertJpegToPdfUsingItextSharp
{
    public partial class Form1 : Form
    {
        int width, height;
        public Form1()
        {
            InitializeComponent();
        }
        
        //浏览图片目录
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dlg.SelectedPath.ToString();
            }
        }

        //选择pdf存储目录
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dlg.SelectedPath.ToString() + "\\OutPDFFile.pdf";
            }
        }

        //开始生成pdf文件
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim())) {
                label1.Text = "请选择图片目录";
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                label1.Text = "请选择PDF存储目录";
                return;
            }
            bool result = Int32.TryParse(textBox3.Text.Trim(), out width);
            if (!result) width = 1920;
            if (width <= 0)
            {
                label1.Text = "Width设置错误，不能小于等于0";
                return;
            }
            result = Int32.TryParse(textBox4.Text.Trim(), out height);
            if (!result) height = 1080;
            if (height <= 0)
            {
                label1.Text = "Height设置错误，不能小于等于0";
                return;
            }

            label1.Text = "开始生成PDF";
            convertJpegToPDFUsingItextSharp(textBox1.Text.Trim(), textBox2.Text.Trim());
            label1.Text = "生成PDF成功";
        }

        
        private void convertJpegToPDFUsingItextSharp(string imagePath, string pdfPath)
        {
            iTextSharp.text.Document Doc = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(width, height), 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(Doc, new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.Read));

            //Open the PDF for writing
            Doc.Open();

            int jpgCount = Directory.GetFiles(imagePath, "*.jpg").Length;
            int convertCount = 0;
            foreach (string F in System.IO.Directory.GetFiles(imagePath, "*.jpg"))
            {
                //Insert a page
                Doc.NewPage();
                //Add image
                Doc.Add(new iTextSharp.text.Jpeg(new Uri(new FileInfo(F).FullName)));
                convertCount++;
                progressBar1.Value = (convertCount * 100) / jpgCount;
            }

            //Close the PDF
            Doc.Close();

        }
    }
}
