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
                server.GeneralMessage(message);
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

                            //var oldMessage = message;
                            //message = String.Format("{0}: {1}", userName, message);

                            //string getChatHistoryCommand = "GetHistoryCommand";
                            //if (message.Contains(getChatHistoryCommand))
                            //{
                            //    foreach (var messageFromHistory in server.mainChannelMessageHistory)
                            //    {
                            //        string[] messageParts = message.Split("|");
                            //        server.SendChatHistory(messageFromHistory, this.ID, messageParts[1]);
                            //        Thread.Sleep(150);
                            //    }
                            //}
                            //else if (message.Contains("GIHC"))
                            //{
                            //    string ID = message.Substring(message.IndexOf("[") + 1,
                            //    message.IndexOf("]") - message.IndexOf("[") - 1);
                            //    ClientObject ChatClient = server.clients.FirstOrDefault(p => p.ID == ID);
                            //    ChatHistoryInfo chat = ChatClient.dialogs.FirstOrDefault(p => p.ID == this.ID);

                            //    if (chat != null)
                            //    {
                            //        foreach (var messageFromHistory in chat.messages)
                            //        {
                            //            string[] messageParts = message.Split("|");
                            //            server.SendChatHistory(messageFromHistory, this.ID, messageParts[1]);
                            //            Thread.Sleep(150);
                            //        }
                            //    }
                            //}
                            //else if (String.Join("", oldMessage.Trim().Take(3)) == "000")
                            //{
                            //    server.GeneralMessage(oldMessage);
                            //    server.mainChannelMessageHistory.Add(oldMessage);
                            //    Console.WriteLine(oldMessage);
                            //}
                            //else if (message.Contains("|"))
                            //{
                            //    string[] messageParts = message.Split("|");
                            //    server.IndividualMessage(messageParts[0], this.ID, messageParts[1]);
                            //    WriteHistoryDialog(messageParts[0], messageParts[1]);
                            //    Console.WriteLine(messageParts[0]);
                            //}
                            //else
                            //{
                            //    server.GeneralMessage(message);
                            //    server.mainChannelMessageHistory.Add(message);
                            //    Console.WriteLine(message);
                            //}
                        }
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.GeneralMessage(message);
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

            //do
            //{
            byte[] lengthData = new byte[4];
            handler.Receive(lengthData);
            length = BitConverter.ToInt32(lengthData);
            byte[] commandData = new byte[1];
            handler.Receive(commandData);
            command = commandData[0];

            bytes = handler.Receive(data, length, SocketFlags.None);
            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            
            //}
            //while (handler.Available > 0);

            return builder.ToString();
        }
        //private int CountEqualNames(string userName)
        //{
        //    int result = 0;
        //    foreach(var t in server.clients)
        //    {
        //        if (t.userName == userName && t.ID != ID)
        //        {
        //            result++;
        //        }
        //    }
        //    return result;
        //}
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
