using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace ChatClient
{
    class ServersFinder
    {
        private string host = "127.0.0.1";
        private int port = 8888;
        private Form1 Form;
        private ComboBox cbServers;
        private const int listenPort = 9119;
        private List<ServerInfo> servers = new List<ServerInfo>();
        Socket listenSocket;
        IPEndPoint groupEP;
        public void FindServersRequest()
        {
            if (listenSocket == null)
            {
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Thread receiveUdp = new Thread(new ThreadStart(ReceiveServersInformation));
                receiveUdp.Start();
            }
            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);
            byte[] ipBytes = ip.GetAddressBytes();
            ipBytes[ipBytes.Length - 1] = 255;
            IPAddress broadcast = new IPAddress(ipBytes);

            List<byte> buffer = new List<byte> { };
            foreach (var t in ip.GetAddressBytes())
            {
                buffer.Add(t);
            }

            foreach (var t in port.ToString())
            {
                buffer.Add(byte.Parse(t.ToString()));
            }

            byte[] sendbuffer = buffer.ToArray();
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);
            listenSocket.SendTo(sendbuffer, ep);
        }
        public void ReceiveServersInformation()
        {
            groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            try
            {
                //Прослушиваем по адресу
                listenSocket.Bind(groupEP);

                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    //адрес, с которого пришли данные
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listenSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listenSocket.Available > 0);
                    AddServer(Encoding.ASCII.GetString(data, 0, data.Length));    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        private void Close()
        {
            if (listenSocket != null)
            {
                listenSocket.Shutdown(SocketShutdown.Both);
                listenSocket.Close();
                listenSocket = null;
            }
        }
        private void AddServer(string server)
        {
            Form.Invoke(new MethodInvoker(() =>
            {
                string[] args = server.Split(',');
                servers.Add(new ServerInfo { Name = args.FirstOrDefault(), Port = args.LastOrDefault() });
                cbServers.DataSource = servers;
                cbServers.DisplayMember = "Name";
            }));
        }
        public ServersFinder(Form1 form, ComboBox cbServers)
        {
            Form = form;
            this.cbServers = cbServers;
        }
    }
}
