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
            bool seEnvia = true;

            if (IPAddress.TryParse(txtIP.Text, out IPAddress ipParse) && !txtIP.Text.Equals(""))
            {
                Ip = ipParse;
                lblDireccion.Text = "IP:";
            }
            else
            {
                lblDireccion.Text = "No es una IP válida";
                seEnvia = false;
            }

            if (int.TryParse(txtPort.Text, out int portParse))
            {
                Puerto = portParse;
                lblPuerto.Text = "PUERTO:";
            }
            else
            {
                lblPuerto.Text = "No es un puerto válido";
                seEnvia = false;
            }

            if (!seEnvia)
            {
                this.DialogResult = DialogResult.None;


            }

        }
    }
}
