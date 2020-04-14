using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ChatClient.Commands
{
    class SendFileCommand : ICommand
    {
        public byte Id => 0;

        public void Run(string message, Client client)
        {
            client.Form.Invoke(new MethodInvoker(() =>
            {
                string fileName = String.Join("", message.Skip(36));
                string fileId = String.Join("", message.Take(36));
                string time = DateTime.Now.ToShortTimeString();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();

                ListViewItem lwItem = new ListViewItem();
                lwItem.Text = time + " " + fileName;
                lwItem.ForeColor = Color.Blue;
                lwItem.Tag = fileId;

                client.tbChat.Items.Add(lwItem);
                client.tbChat.Items.Add(IP);
                client.tbChat.Items.Add("");

                client.ReceivedFiles.Add(new ReceivedFileInfo()
                {
                    Name = fileName,
                    Id = fileId,
                    LWItem = lwItem
                });
                //tbChat.Text = "\r\n" + tbChat.Text;
                //tbChat.Text = IP + "\r\n" + tbChat.Text;
                //tbChat.Text = time + " " + message + "\r\n" + tbChat.Text + "\r\n";
            }));
        }
    }
}
