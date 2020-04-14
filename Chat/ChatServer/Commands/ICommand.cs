using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Commands
{
    interface ICommand
    {
        byte Id { get; }
        void Run(string message, ServerObject server);
    }
}
