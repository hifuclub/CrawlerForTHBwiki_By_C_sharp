using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCrawler
{
    delegate string Crawler(string url);
    public partial class Form1 : Form
    {
        Crawler crawler;
        bool isFristChangeNews = true;
        public static bool isNoNewForm2 = true;
        Thread threadForCrawlers;
        bool isCloseChildThread;
        public Form1()
        {
            InitializeComponent();
            myIni();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            ChangeNews("爬取中");
            string tempNews = crawler(textBox1.Text);
            ChangeNews(tempNews);

        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            if (TopMost)
            {
                button3.Text = "▼";
            }
            else {
                button3.Text = "▲";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!isNoNewForm2) {
                foreach (Form form in TestCrawlers.form2ArrayList)
                {
                    if (!form.IsDisposed)
                    {
                        form.Dispose();
                    }
                }
                isNoNewForm2 = true;
                TestCrawlers.form2ArrayList.Clear();
                ChangeNews("释放所有窗口");
            }
        }

        public void ChangeNews(string news) {
            if (isFristChangeNews)
            {
                label2.Text = "";
                isFristChangeNews = false;
            }
            label8.Text = label7.Text;
            label7.Text = label6.Text;
            label6.Text = label5.Text;
            label5.Text = label2.Text;
            label2.Text = DateTime.Now + "  " + news;
        }

        private void myIni()
        {
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            threadForCrawlers = new Thread(myStaticThreadMethod);
            threadForCrawlers.Start();
           
        }
        void myStaticThreadMethod()
        {

            TestCrawlers testCrawlers = new TestCrawlers();
            crawler += testCrawlers.crawler;
            while (!isCloseChildThread)
            {

            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isCloseChildThread = true;//销毁子线程
        }
    }
}
