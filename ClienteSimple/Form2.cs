using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteSimple
{
    public partial class Form2 : Form
    {
        public IPAddress Ip;
        public int Puerto;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ip = IPAddress.Parse(txtIP.Text);
            Puerto = int.Parse(txtPort.Text);
            this.Close();
        }
    }
}
