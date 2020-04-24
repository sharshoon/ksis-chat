using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChatServer.Commands
{
    class GetHistoryCommand : ICommand
    {
        public byte Id => 3;
        ClientObject Client;

        public void Run(string message, ServerObject server)
        {
            foreach (var messageFromHistory in server.mainChannelMessageHistory)
            {
                server.SendGeneralHistory(messageFromHistory.Message,messageFromHistory.Command, Client.ID, message);
                Thread.Sleep(150);
            }
        }
        public GetHistoryCommand(ClientObject client)
        {
            Client = client;
        }
    }
}
