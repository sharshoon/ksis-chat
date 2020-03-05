using System;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class ClientSubj
    {
        protected internal string ID { get; private set; }
        internal Socket handler;
        internal string userName; 
        ServerSubj server;

        public ClientSubj(Socket handler, ServerSubj serverObject)
        {
            ID = Guid.NewGuid().ToString();
            this.handler = handler;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                string message = GetMessage();
                userName = message;
                if (CountEqualNames(userName) != 0)
                {
                    userName = userName + $"({CountEqualNames(userName).ToString()})";
                }
                message = userName + " вошел в чат!!!";
                server.BroadcastMessage(message, this.ID);
                Console.WriteLine(message);
                
                while(true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
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
                server.RemoveConnection(this.ID);
                Close();
            }
        }
        private string GetMessage()
        {
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[256]; 

            do
            {
                bytes = handler.Receive(data);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (handler.Available > 0);

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
            if (handler != null)
                 handler.Close();
        }
    }
}
