using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChatServer.Commands
{
    class GetIndividualHistoryCommand : ICommand
    {
        public byte Id => 4;
        ClientObject Client;

        public void Run(string message, ServerObject server)
        {
            string ID = String.Join("", message.Take(36));

            ClientObject ChatClient = server.clients.FirstOrDefault(p => p.ID == ID);
            ChatHistoryInfo chat = ChatClient.dialogs.FirstOrDefault(p => p.ID == Client.ID);

            if (chat != null)
            {
                foreach (var messageFromHistory in chat.messages)
                {
                    server.SendChatHistory(messageFromHistory, Client.ID, String.Join("", message.Skip(36)));
                    Thread.Sleep(150);
                }
            }
        }
        public GetIndividualHistoryCommand(ClientObject client)
        {
            Client = client;
        }
}
}
