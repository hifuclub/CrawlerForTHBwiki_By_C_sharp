using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace TestCrawler
{
    public partial class Form2 : Form
    {
        string ls = "";
        ArrayList albumInfoArray;
        int formNum;
        public Form2(ArrayList albumInfoArray)
        {
            InitializeComponent();
            
            this.albumInfoArray = albumInfoArray;
            foreach (string s in albumInfoArray)
            {
                ls = ls + s + "\r\n";
            }
            textBox1.Text = ls;
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text, true);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //if (result == DialogResult.OK)
            //{
            //    e.Cancel = false;  //点击OK   
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }
    }
}
