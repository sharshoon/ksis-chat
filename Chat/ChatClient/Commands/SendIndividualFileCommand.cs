using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient.Commands
{
    class SendIndividualFileCommand : ICommand
    {
        public byte Id => 6;

        public void Run(string message, Client user)
        {
            UserInfo client = user.users.FirstOrDefault(p => p.ID == message.Substring(message.IndexOf("[") + 1,
                                message.IndexOf("]") - message.IndexOf("[") - 1));
            user.Form.Invoke(new MethodInvoker(() =>
            {
                if (user.cbChooseUser.Items.Contains(client) && user.cbChooseUser.Text != client.Name)
                {
                    user.cbChooseUser.Items.Add(new UserInfo { Name = client.Name + "[new message]", ID = client.ID });
                }
                else if (user.cbChooseUser.Text == client.Name)
                {
                    // Пропускает ID отправителя и ID файла
                    string fileName = String.Join("", message.Skip(38 + 36));
                    
                    // Берет ID файла
                    string fileId = String.Join("", message.Skip(38).Take(36));
                    string time = DateTime.Now.ToShortTimeString();
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();

                    ListViewItem lwItem = new ListViewItem();
                    lwItem.Text = time + " " + fileName;
                    lwItem.ForeColor = Color.Blue;
                    lwItem.Tag = fileId;

                    user.tbChat.Items.Add(lwItem);
                    user.tbChat.Items.Add(IP);
                    user.tbChat.Items.Add("");

                    user.ReceivedFiles.Add(new ReceivedFileInfo()
                    {
                        Name = fileName,
                        Id = fileId,
                        LWItem = lwItem
                    });
                }
                user.cbChooseUser.Items.Remove(client);
            }));
        }
    }
}
