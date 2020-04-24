using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer.Commands
{
    class SendIndividualFileCommand : ICommand
    {
        public byte Id => 6;
        ClientObject Client;

        public void Run(string message, ServerObject server)
        {
            var receiverID = message.Take(36);
            var messageBody = String.Join("", message.Skip(36));

           
            server.SendIndividualFile(messageBody, Client.ID, String.Join("", receiverID));
            Client.WriteHistoryDialog(Encoding.UTF8.GetString(new byte[] { 0 }) + messageBody, String.Join("", receiverID));
            Console.WriteLine(messageBody);
        }
        public SendIndividualFileCommand(ClientObject client)
        {
            Client = client;
        }
    }
}
