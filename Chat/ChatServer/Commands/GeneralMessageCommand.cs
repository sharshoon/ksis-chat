using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Commands
{
    class GeneralMessageCommand : ICommand
    {
        public byte Id { get; } = 1;

        public void Run(string message, ServerObject server)
        {
            server.GeneralMessage(message, new byte[] { 1 });
            server.mainChannelMessageHistory.Add(message);
            Console.WriteLine(message);
        }
        public GeneralMessageCommand()
        { }
    }
}
