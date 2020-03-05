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
    class Broadcast
    {
        private string host = "127.0.0.1";
        private int port = 8888;
        private Form1 Form;
        private ComboBox cbServers;
        private const int listenPort = 9119;
        private List<ServerInfo> servers = new List<ServerInfo>();
        UdpClient listener;

        public void BroadCastRequest()
        {
            if (listener == null)
            {
                listener = new UdpClient(listenPort);
            }

            Thread receiveUdp = new Thread(new ThreadStart(ReceiveUdpMessage));
            receiveUdp.Start();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);
            byte[] ipBytes = ip.GetAddressBytes();
            ipBytes[ipBytes.Length - 1] = 255;
            IPAddress broadcast = new IPAddress(ipBytes);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            List<byte> buf = new List<byte> { };
            foreach (var t in ip.GetAddressBytes())
            {
                buf.Add(t);
            }

            foreach (var t in port.ToString())
            {
                buf.Add(byte.Parse(t.ToString()));
            }

            byte[] sendbuf = buf.ToArray();
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);
            s.SendTo(sendbuf, ep);
        }
        public void ReceiveUdpMessage()
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    AddServer(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
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
        public Broadcast(Form1 form, ComboBox cbServers)
        {
            Form = form;
            this.cbServers = cbServers;
        }
    }
}
