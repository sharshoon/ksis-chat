using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace ChatServer
{
    class ServerSubj
    {
        static TcpListener tcpListener;
        List<ClientSubj> clients = new List<ClientSubj>();

        static IPAddress remoteAddress; // хост для отправки данных
        const int remotePort = 8888; // порт для отправки данных
        const int localPort = 8888; // локальный порт для прослушивания входящих подключений

        protected internal void AddConnection(ClientSubj clientSubj)
        {
            clients.Add(clientSubj);
        }
        protected internal void RemoveConnection(string id)
        {
            ClientSubj client = clients.FirstOrDefault(c => c.ID == id);

            if (client != null)
            {
                clients.Remove(client);
            }
        }
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, localPort);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientSubj clientObject = new ClientSubj(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        protected internal void ReceiveUdpMessage()
        {
            int listenPort = 11000;

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    //Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                    int remoteUdpPort = 9119;
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPEndPoint ep = new IPEndPoint(groupEP.Address, remoteUdpPort);

                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();
                    byte[] sendbuf = Encoding.ASCII.GetBytes(IP + ","+ remotePort.ToString());
                    s.SendTo(sendbuf, ep);
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
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Stream.Write(data, 0, data.Length);
                //if (clients[i].ID != id)
                //{
                //}
            }
        }
        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }
    }
}
