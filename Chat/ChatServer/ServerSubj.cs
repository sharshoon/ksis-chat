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
        static Socket listenSocket;
        internal List<ClientSubj> clients = new List<ClientSubj>();

        static IPAddress remoteAddress;
        const int remotePort = 8888;
        const int localPort = 8888;

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
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, localPort);

            // создаем сокет
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    ClientSubj clientObject = new ClientSubj(handler, this);
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
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            try
            {
                //Прослушиваем по адресу
                listeningSocket.Bind(groupEP);

                while (true)
                {
                    //Console.WriteLine("Waiting for broadcast");
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    //адрес, с которого пришли данные
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);
                    // получаем данные о подключении
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                    int remoteUdpPort = 9119;
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    
                    IPEndPoint ep = new IPEndPoint(remoteFullIp.Address, remoteUdpPort);

                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();
                    byte[] sendbuf = Encoding.ASCII.GetBytes(IP + "," + remotePort.ToString());
                    s.SendTo(sendbuf, ep);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listeningSocket != null)
                {
                    listeningSocket.Shutdown(SocketShutdown.Both);
                    listeningSocket.Close();
                    listeningSocket = null;
                }
            }
        }
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].handler.Send(data);
            }
        }
        protected internal void IndividualMessage(string message, string id, string userName)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            ClientSubj client = clients.FirstOrDefault(p => p.userName.Trim() == userName.Trim());
            client?.handler.Send(data);

            if (client != null)
            {
                clients.FirstOrDefault(p => p.ID == id)
                    .handler.Send(data);
            }
            else
            {
                clients.FirstOrDefault(p => p.ID == id)
                    .handler.Send(Encoding.Unicode.GetBytes("Такого пользователя не существует"));
            }
        }
        protected internal void Disconnect()
        {
            listenSocket.Close();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }
    }
}
