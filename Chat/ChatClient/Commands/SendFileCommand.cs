using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Commands
{
    class SendFileCommand : ICommand
    {
        public byte Id => 0;

        public void Run(string message, Client client)
        {
            MessageBox.Show("дошло");
        }
    }
}
