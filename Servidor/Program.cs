using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servidor
{
    internal class Program
    {
        public bool ServerRunning { set; get; } = true;
        private Socket? s;
        bool puertoLibre = true;

        public void InitServer()
        {
            int port = 31416;



            while (puertoLibre)
            {

                try
                {
                    IPEndPoint ie = new IPEndPoint(IPAddress.Any, port);
                    s = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
            ProtocolType.Tcp);
                    s.Bind(ie);

                    Console.WriteLine($"Port {port} free");

                    s.Listen(10);
                    puertoLibre = false;

                    Console.WriteLine($"Servidor iniciado. Escuchando en {ie.Address}:{ie.Port}");
                    Console.WriteLine("Esperando conexiones... (Ctrl+C para salir)");
                    while (ServerRunning)
                    {

                        Socket sClient = s.Accept();
                        Thread hilo = new Thread(() => ClientDispatcher(sClient));
                        hilo.IsBackground = true;
                        hilo.Start();

                    }


                }

                catch (SocketException e) when (e.ErrorCode ==
                    (int)SocketError.AddressAlreadyInUse)
                {

                    Console.WriteLine($"Port {port} in use");
                    port++;
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Fin");
                }

            }

        }

        private void ClientDispatcher(Socket sClient)

        {
            using (sClient) // Este using por separado pues luego irá en un hilo
            {

                IPEndPoint ieClient = (IPEndPoint)sClient.RemoteEndPoint;
                Console.WriteLine($"Cliente conectado:{ieClient.Address} en puerto {ieClient.Port}");

                //telnet 127.0.0.1 31416 
                Encoding codificacion = Console.OutputEncoding;
                using (NetworkStream ns = new NetworkStream(sClient))
                using (StreamReader sr = new StreamReader(ns, codificacion))
                using (StreamWriter sw = new StreamWriter(ns, codificacion))
                {
                    //sr.BaseStream.ReadTimeout = 2000;
                    sw.AutoFlush = true;
                    string welcome = "Bienvenido al servidor de Carril";
                    sw.WriteLine(welcome);
                    string? msg = "";


                    try
                    {

                        msg = sr.ReadLine();
                        if (msg != null)
                        {
                            if (msg.ToLower() == "time")
                            {
                                msg = DateTime.Now.ToString("HH:mm:ss");
                                sw.WriteLine($"La hora es {msg}");

                            }
                            if (msg.ToLower() == "date")
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd");
                                sw.WriteLine($"La fecha es {msg}");

                            }
                            if (msg.ToLower() == "all")
                            {
                                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                sw.WriteLine($"Fecha y hora {msg}");

                            }
                            if (msg.StartsWith("close "))
                            {
                                string linea = "";
                                string directorio = Environment.GetEnvironmentVariable("PROGRAMDATA");
                                string ruta = directorio + "\\contraseñas.txt";
                                using (StreamReader sw2 = new StreamReader(ruta))
                                    try
                                    {
                                        linea = sw2.ReadLine();
                                    }
                                    catch (IOException)
                                    {
                                        Console.WriteLine("Error leyendo el archivo de contraseñas");
                                    }
                                if (msg.Trim().Contains(linea))
                                {
                                    
                                    s.Close();
                                }
                                else if (msg.Trim().Length <= 5)
                                {
                                    sw.WriteLine("No se ha enviado contraseña alguna");
                                }
                                else
                                {

                                    sw.WriteLine("Contraseña incorrecta");
                                }


                            }

                        }
                        else
                        {

                        }
                    }
                    catch (IOException)
                    {
                        msg = null;
                    }



                    Console.WriteLine("Cliente desconectado.\nConexión cerrada");
                }
            }
        }





    }
}
