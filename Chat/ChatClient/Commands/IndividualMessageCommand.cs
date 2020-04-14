using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Commands
{
    class IndividualMessageCommand : ICommand
    {
        public byte Id => 2;

        public void Run(string message, Client user)
        {
            UserInfo client = user.users.FirstOrDefault(p => p.ID == message.Substring(message.IndexOf("[") + 1,
                                message.IndexOf("]") - message.IndexOf("[") - 1));
            user.Form.Invoke(new MethodInvoker(() =>
            {
                if (user.cbChooseUser.Items.Contains(client))
                {
                    user.cbChooseUser.Items.Add(new UserInfo { Name = client.Name + "[new message]", ID = client.ID });
                }
                user.cbChooseUser.Items.Remove(client);
            }));
        }
    }
}
