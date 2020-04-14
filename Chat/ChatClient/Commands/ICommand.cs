using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    interface ICommand
    {
        byte Id { get; }
        void Run(string message, Client client);
    }
}
