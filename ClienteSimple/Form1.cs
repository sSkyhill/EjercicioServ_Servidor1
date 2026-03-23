using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteSimple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int puerto = 31416;
        private async Task<string> EnvioYRecepcionAsync(string comando)
        {
            try
            {
                using (Socket conexion = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint ep = new IPEndPoint(ip, puerto);

                    await conexion.ConnectAsync(ep);

                    Encoding codificacion = Console.OutputEncoding;
                    using (NetworkStream ns = new NetworkStream(conexion))
                    using (StreamReader sr = new StreamReader(ns, codificacion))
                    using (StreamWriter sw = new StreamWriter(ns, codificacion))
                    {
                        sw.AutoFlush = true;

                        string msg = await sr.ReadLineAsync();
                        if (txtContrasena.Text == null)
                        {
                            return "La contraseña no puede estar vacía";
                        }
                        else
                        {
                            await sw.WriteLineAsync(comando + txtContrasena.Text);
                        }

                        msg = await sr.ReadLineAsync();
                        return msg;
                    }
                }
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.GetType().Name}. Contacte con soporte.";
            }
        }


        private async void btnDate_Click(object sender, EventArgs e)
        {
            lblContrasena.Text = await EnvioYRecepcionAsync("date");

        }

        private async void btnTime_Click(object sender, EventArgs e)
        {
            lblContrasena.Text = await EnvioYRecepcionAsync("time");
        }

        private async void btnAll_Click(object sender, EventArgs e)
        {
            lblContrasena.Text = await EnvioYRecepcionAsync("all");
        }

        private async void btnClose_Click(object sender, EventArgs e)
        {
            lblContrasena.Text = await EnvioYRecepcionAsync("close ");
        }

        private void btnServ_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            ip = form2.Ip;
            puerto = form2.Puerto;
        }
    }
}
