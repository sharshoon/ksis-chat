using System;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class ClientSubj
    {
        protected internal string ID { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        internal string userName;
        TcpClient client;
        ServerSubj server;

        public ClientSubj(TcpClient tcpClient, ServerSubj serverObject)
        {
            ID = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;
                if (CountEqualNames(userName) != 0)
                {
                    userName = userName + $"({CountEqualNames(userName).ToString()})";
                }
                message = userName + " вошел в чат!!!";
                server.BroadcastMessage(message, this.ID);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        //Console.WriteLine(message);
                        if (message.Contains("|"))
                        {
                            string[] messageParts = message.Split("|");
                            server.IndividualMessage(messageParts[0], this.ID, messageParts[1]);
                        }
                        else
                        {
                            server.BroadcastMessage(message, this.ID);
                        }
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.ID);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.ID);
                Close();
            }
        }
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }
        private int CountEqualNames(string userName)
        {
            int result = 0;
            foreach(var t in server.clients)
            {
                if (t.userName == userName && t.ID != ID)
                {
                    result++;
                }
            }
            return result;
        }
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
