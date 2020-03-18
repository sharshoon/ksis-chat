using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;


namespace ChatClient
{
    class Client
    {
        private bool alive = false;
        private static string userName;
        private string host = "127.0.0.1";
        private int port = 8888;
        private Socket socket;
        private TextBox tbChat;
        private ComboBox cbChooseUser;
        private Form1 Form;
        private List<UserInfo> users = new List<UserInfo>();

        public void Login(string name, string ip, string port)
        {
            userName = name;
            this.host = ip;
            this.port = int.Parse(port);
           
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(this.host.ToString()), this.port);
 
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                // Создать тут новый сокет про юзеров

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                socket.Connect(ipPoint);
                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SendMessage(TextBox tbMessage)
        {
            try
            {
                string message = cbChooseUser.Text.Trim() == "" ? 
                    String.Format(tbMessage.Text) :
                    String.Format(tbMessage.Text + "|" + (((UserInfo)cbChooseUser.SelectedItem).ID));
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
                tbMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ReceiveMessage()
        {
            alive = true;
            while (alive)
            {
                try
                {
                    byte[] data = new byte[10000]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; 

                    //do
                   // {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (socket.Available > 0);

                    string message = builder.ToString();

                    if (message.Contains('|'))
                    {
                        string[] messageParts = message.Split('|');
                        ChangeUsersList(messageParts);
                    }
                    else if (message.Contains("["))
                    {
                        UserInfo client = users.FirstOrDefault(p => p.ID == message.Substring(message.IndexOf("[") + 1, 
                            message.IndexOf("]") - message.IndexOf("[") - 1));
                        Form.Invoke(new MethodInvoker(() =>
                        {
                            if (cbChooseUser.Items.Contains(client))
                            {
                                cbChooseUser.Items.Add(new UserInfo { Name = client.Name + "[new message]", ID = client.ID });
                            }
                            cbChooseUser.Items.Remove(client);
                        }));
                    }
                    else
                    {
                        Form.Invoke(new MethodInvoker(() =>
                        {
                            string time = DateTime.Now.ToShortTimeString();
                            var host = Dns.GetHostEntry(Dns.GetHostName());
                            string IP = host.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork).ToString();

                            tbChat.Text = "\r\n" + tbChat.Text;
                            tbChat.Text = IP + "\r\n" + tbChat.Text;
                            tbChat.Text = time + " " + message + "\r\n" + tbChat.Text + "\r\n";
                        }));
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Подключение прервано!");
                    Disconnect();
                }
            }
        }
        
        private void ChangeUsersList(string[] clients)
        {
            users.Clear();
            foreach (var client in clients)
            {
                string[] messageParts = client.Split(',');
                if (messageParts.Length == 2)
                {
                    users.Add(new UserInfo
                    {
                        Name = messageParts[0],
                        ID = messageParts[1]
                    });
                }
            }
            PrintUsersList();
        }
        private void PrintUsersList()
        {
            Form.Invoke(new MethodInvoker(() =>
            {
                cbChooseUser.Items.Clear();
                cbChooseUser.Items.AddRange(users.ToArray());
                cbChooseUser.DisplayMember = "Name";
            }));
        }

        public void Disconnect()
        {
            alive = false;
            if (socket != null)
                socket.Close();//отключение потока
            Environment.Exit(0); //завершение процесса
        }

        public void GetDialogHistory()
        {
            try
            {
                string message = "GetHistoryCommand" + "|" + users.FirstOrDefault(p => p.Name == userName).ID;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GetIndividualDialogHistory()
        {
            try
            {
                string message = $"[{(((UserInfo)cbChooseUser.SelectedItem).ID)}]"+"GIHC" + "|" + users.FirstOrDefault(p => p.Name == userName).ID;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                if (cbChooseUser.Text.Contains("["))
                {
                    UserInfo selected = (UserInfo)cbChooseUser.SelectedItem;
                    cbChooseUser.Items.RemoveAt(cbChooseUser.SelectedIndex);

                    UserInfo client = users.FirstOrDefault(p => p.ID == selected.ID);
                    cbChooseUser.Items.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Client(TextBox tbChat, Form1 form, ComboBox cbChooseUser)
        {
            Form = form;
            this.tbChat = tbChat;
            this.cbChooseUser = cbChooseUser;
        }
    }
}
