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
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text, true);
        }
    }
}
