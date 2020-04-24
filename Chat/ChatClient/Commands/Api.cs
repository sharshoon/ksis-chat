using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Commands
{
    class Api
    {
        private Dictionary<byte, ICommand> commands { get; set; }
        public ICommand GetCommand(byte id)
        {
            return commands[id];
        }
        public Api()
        {
            commands = new Dictionary<byte, ICommand>();
            commands.Add(0, new SendFileCommand());
            commands.Add(1, new GeneralMessageCommand());
            commands.Add(2, new IndividualMessageCommand());
            commands.Add(5, new ChangeUsersListCommand());
            commands.Add(6, new SendIndividualFileCommand());
        }
    }
}
