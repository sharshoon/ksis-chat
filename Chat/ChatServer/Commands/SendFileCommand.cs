using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer.Commands
{
    class SendFileCommand : ICommand
    {
        public byte Id { get; } = 0;

        public void Run(string message, ServerObject server)
        {
            server.GeneralMessage(message);
            server.mainChannelMessageHistory.Add(message);
            Console.WriteLine(message);
        }
        public SendFileCommand()
        { }
    }
}
