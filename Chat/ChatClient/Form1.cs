using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        bool alive = false;
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userName = tbName.Text;
            client = new TcpClient();
            try
            {
                client.Connect(tbIP.Text.Trim(), int.Parse(tbPort.Text.Trim())); //подключение клиента
                stream = client.GetStream(); // получаем поток

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ReceiveMessage()
        {
            alive = true;
            while (alive)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    this.Invoke(new MethodInvoker(() =>
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        var host = Dns.GetHostEntry(Dns.GetHostName());
                        string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();
                        
                        tbChat.Text = "\r\n" + tbChat.Text;
                        tbChat.Text = IP + "\r\n" + tbChat.Text;
                        tbChat.Text = time + " " + message + "\r\n" + tbChat.Text + "\r\n";
                    }));

                }
                catch
                {
                    MessageBox.Show("Подключение прервано!");
                    Disconnect();
                }
            }
        } 
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = String.Format(tbMessage.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                tbMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Disconnect()
        {
            alive = false;
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            //Environment.Exit(0); //завершение процесса
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);
            byte[] ipBytes = ip.GetAddressBytes();
            ipBytes[ipBytes.Length - 1] = 255;
            ip = new IPAddress(ipBytes);

            IPAddress broadcast = ip;

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //IPAddress broadcast = IPAddress.Parse("192.168.43.255"); // !!!!!!!!!!!!

            byte[] sendbuf = Encoding.ASCII.GetBytes("test");
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);

            s.SendTo(sendbuf, ep);
        }
    }
}
