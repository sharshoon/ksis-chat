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
            var messageBody = String.Join("",message.Skip(36));

            messageBody = String.Format("{0}: {1}", Client.userName, messageBody);

            server.IndividualMessage(messageBody, Client.ID, String.Join("",receiverID));
            Client.WriteHistoryDialog(Encoding.UTF8.GetString(new byte[] { 1 }) + messageBody, String.Join("", receiverID));
            Console.WriteLine(messageBody);
        }
        public IndividualMessageCommand(ClientObject client)
        {
            Client = client;
        }
    }
}
