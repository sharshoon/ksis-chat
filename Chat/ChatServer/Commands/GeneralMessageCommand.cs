using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Commands
{
    class GeneralMessageCommand : ICommand
    {
        public byte Id { get; } = 1;
        ClientObject Client;

        public void Run(string message, ServerObject server)
        {
            message = String.Format("{0}: {1}", Client.userName, message);
            server.GeneralMessage(message, new byte[] { 1 });
            server.mainChannelMessageHistory.Add(new MessageHistory
            {
                Message = message,
                Command = new byte[] { 1 }
            });
            Console.WriteLine(message);
        }
        public GeneralMessageCommand(ClientObject client)
        {
            Client = client;
        }
    }
}
