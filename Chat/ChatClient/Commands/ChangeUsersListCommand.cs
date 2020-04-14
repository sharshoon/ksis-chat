using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    class ChangeUsersListCommand : ICommand
    {
        public byte Id => 5;

        public void Run(string message, Client client)
        {
            string[] messageParts = message.Split('|');
            client.ChangeUsersList(messageParts);
        }
    }
}
