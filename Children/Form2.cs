using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Children
{
    public partial class Form2 : Form
    {
        public byte level; 

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            level = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            level = 3;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            level = 1;
        }
    }
}
