using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Commands
{
    class GeneralMessageCommand : ICommand
    {
        public byte Id => 0;

        public void Run(string message, Client client)
        {
            client.Form.Invoke(new MethodInvoker(() =>
            {
                string time = DateTime.Now.ToShortTimeString();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();

                client.tbChat.Items.Add(time + " " + message);
                client.tbChat.Items.Add(IP);
                client.tbChat.Items.Add("");
                //tbChat.Text = "\r\n" + tbChat.Text;
                //tbChat.Text = IP + "\r\n" + tbChat.Text;
                //tbChat.Text = time + " " + message + "\r\n" + tbChat.Text + "\r\n";
            }));
        }
    }
}
