using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using ChatServer.Commands;

namespace ChatServer
{
    class ClientObject
    {
        protected internal string ID { get; private set; }
        internal Socket handler;
        internal string userName;
        internal List<ChatHistoryInfo> dialogs;
        ServerObject server;

        public ClientObject(Socket handler, ServerObject serverObject)
        {
            ID = Guid.NewGuid().ToString();
            this.handler = handler;
            server = serverObject;
            dialogs = new List<ChatHistoryInfo>();
        }

        public void Process()
        {
            try
            {
                byte command;
                int length;
                string message = GetMessage(out length, out command);
                
                userName = message;
                message = userName + " вошел в чат";
                
                server.AddConnection(this);
                //Thread.Sleep(1000);
                server.GeneralMessage(message, new byte[] { 1 });
                server.mainChannelMessageHistory.Add(message);
                //Thread.Sleep(300);

                Console.WriteLine(message);
                
                while (true)
                {
                    try
                    {
                        Api api = new Api(this);
                        message = GetMessage(out length, out command);

                        ICommand commandRunner = api.GetCommand(command);
                        if (commandRunner != null)
                        {
                            commandRunner.Run(message, server);
                        }
                        else
                        {
                            // Ошибка, нет такой команды
                            throw new Exception("Неопознанная команда");
                        }
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.GeneralMessage(message, new byte[] { 1 });
                        server.mainChannelMessageHistory.Add(message);
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
        private string GetMessage(out int length, out byte command)
        {
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[10000]; // tut

            byte[] lengthData = new byte[4];
            handler.Receive(lengthData);
            length = BitConverter.ToInt32(lengthData);
            byte[] commandData = new byte[1];
            handler.Receive(commandData);
            command = commandData[0];

            bytes = handler.Receive(data, length, SocketFlags.None);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
           
            return builder.ToString();
        }
        
        public void WriteHistoryDialog(string message, string ID)
        {
            ChatHistoryInfo dialog = this.dialogs.FirstOrDefault(p => p.ID == ID);
            if(dialog == null)
            {
                dialogs.Add(new ChatHistoryInfo
                {
                    ID = ID,
                    messages = new List<string>() { message }
                });
            }
            else
            {
                dialog.messages.Add(message);
            }
        }
        protected internal void Close()
        {
            if (handler != null)
                 handler.Close();
        }
    }
}
