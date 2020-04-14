using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ChatServer
{
    class ServerObject
    {
        static Socket listenSocket;
        internal ObservableCollection<ClientObject> clients = new ObservableCollection<ClientObject>();
        internal List<string> mainChannelMessageHistory = new List<string>();

        static IPAddress remoteAddress;
        const int remotePort = 8888;
        const int localPort = 8888;
        static object locker = new object();
        internal void AddConnection(ClientObject clientSubj)
        {
            clients.Add(clientSubj);
        }
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.ID == id);

            if (client != null)
            {
                clients.Remove(client);
            }
        }
        internal void Listen()
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

                clients.CollectionChanged += ClientsCollectionChanged;

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    ClientObject clientObject = new ClientObject(handler, this);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
                ProgramExit();
            }
        }
        private void ClientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string users = "";
            foreach(var client in clients)
            {
                if (client.userName != null)
                {
                    users += client.userName + "," + client.ID + "|";   
                }
            }

            byte[] command = { 5 };
            byte[] data = Encoding.Unicode.GetBytes(users);
            byte[] length = BitConverter.GetBytes(data.Length);
            byte[] fulldata = length.Concat(command).Concat(data).ToArray();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i]?.handler.Send(fulldata);
            }
        }
        internal void ReceiveUdpMessage()
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
        internal void GeneralMessage(string message, byte[] command)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            byte[] length = BitConverter.GetBytes(data.Length);
            byte[] fulldata = length.Concat(command).Concat(data).ToArray();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i]?.handler.Send(fulldata);
            }
        }
        internal void IndividualMessage(string message, string IDSender, string IDReceiver)
        {
            byte[] command = new byte[] { 2 };
            byte[] data = Encoding.Unicode.GetBytes($"[{IDSender}]"+message);
            byte[] length = BitConverter.GetBytes(data.Length);
            byte[] fulldata = length.Concat(command).Concat(data).ToArray();
            ClientObject client = clients.FirstOrDefault(p => p.ID.Trim() == IDReceiver.Trim());
            client?.handler.Send(fulldata);

            if (client != null && (IDSender != IDReceiver))
            {
                byte[] commandGeneral = new byte[] { 1 };
                byte[] dataGeneral = Encoding.Unicode.GetBytes(message);
                byte[] lengthGeneral = BitConverter.GetBytes(dataGeneral.Length);
                byte[] fulldataGeneral = lengthGeneral.Concat(commandGeneral).Concat(dataGeneral).ToArray();
                clients.FirstOrDefault(p => p.ID == IDSender)
                    .handler.Send(fulldataGeneral); /// ????????????
            }
            else if (client == null)
            {
                clients.FirstOrDefault(p => p.ID == IDSender)
                    .handler.Send(Encoding.Unicode.GetBytes("Такого пользователя не существует"));
            }
        }
        internal void SendChatHistory(string message, string IDSender, string IDReceiver)
        {
            byte[] command = new byte[] { 1 };
            byte[] data = Encoding.Unicode.GetBytes(message);
            byte[] length = BitConverter.GetBytes(data.Length);
            byte[] fulldata = length.Concat(command).Concat(data).ToArray();
            clients.FirstOrDefault(p => p.ID == IDSender)
                    .handler.Send(fulldata);
            Console.WriteLine(message);
        }
        internal void SendMainChannelMessageHistory()
        {
            foreach (var message in mainChannelMessageHistory)
            {
                byte[] command = new byte[] { 1 };
                byte[] data = Encoding.Unicode.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] fulldata = length.Concat(command).Concat(data).ToArray();
                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].handler.Send(fulldata);
                }
            }
        }
        internal void Disconnect()
        {
            listenSocket.Close();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
        }
        internal void ProgramExit()
        {
            Environment.Exit(0);
        }
    }
}
