using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer.Commands
{
    internal class Api
    {
        public ClientObject Client;
        private Dictionary<byte, ICommand> commands { get; set; }
        public ICommand GetCommand(byte id)
        {
            return commands[id];
        }
        public Api(ClientObject client)
        {
            Client = client;

            commands = new Dictionary<byte, ICommand>();
            commands.Add(0, new SendFileCommand());
            commands.Add(1, new GeneralMessageCommand(client));
            commands.Add(2, new IndividualMessageCommand(client));
            commands.Add(3, new GetHistoryCommand(client));
            commands.Add(4, new GetIndividualHistoryCommand(client));
            commands.Add(6, new SendIndividualFileCommand(client));
        }
    }
}
