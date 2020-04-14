using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer.Commands
{
    class IndividualMessageCommand : ICommand
    {
        public byte Id => 2;
        ClientObject Client;

        public void Run(string message, ServerObject server)
        {
            var receiverID = message.Take(36);
            var messageBody = message.Skip(36);
            server.IndividualMessage(String.Join("",messageBody), Client.ID, String.Join("",receiverID));
            Client.WriteHistoryDialog(String.Join("", messageBody), String.Join("", receiverID));
            Console.WriteLine(String.Join("", messageBody));
        }
        public IndividualMessageCommand(ClientObject client)
        {
            Client = client;
        }
    }
}
