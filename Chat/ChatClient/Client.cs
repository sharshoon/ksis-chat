using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Drawing;
using ChatClient.Commands;
using static System.Windows.Forms.ListView;

namespace ChatClient
{
    class Client
    {
        private bool alive = false;
        private static string userName;
        private string host = "127.0.0.1";
        private int port = 8888;
        public Socket socket;
        public ListView tbChat;
        public ComboBox cbChooseUser;
        public Form1 Form;
        public List<UserInfo> users = new List<UserInfo>();
        public string Id { get; set; }
        public List<FileUploadResult> PinedFiles = new List<FileUploadResult>();
        public List<FileUploadResult> SentFiles = new List<FileUploadResult>();
        public List<ReceivedFileInfo> ReceivedFiles = new List<ReceivedFileInfo>();
        public string fileServiceUrl = "http://localhost:8889/connection/";//"https://localhost:44328";

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
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] command = { 1 };
                byte[] fulldata = length.Concat(command).Concat(data).ToArray();
                socket.Send(fulldata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SendMessage(RichTextBox tbMessage)
        {
            try
            {
                foreach (var file in PinedFiles)
                {
                    if (tbMessage.Text.Contains($"{file.fileName}"))
                    {
                        byte[] fileCommand = new byte[] { 0 };
                        byte[] filedata = Encoding.Unicode.GetBytes(PinedFiles.FirstOrDefault(p => p.fileName == file.fileName).Id + file.fileName);
                        byte[] fileDataLength = BitConverter.GetBytes(filedata.Length);
                        byte[] fullFileData = fileDataLength.Concat(fileCommand).Concat(filedata).ToArray();
                        socket.Send(fullFileData);

                        SentFiles.Add(file);

                        var newMessage = tbMessage.Text.Remove(tbMessage.Text.IndexOf(file.fileName), file.fileName.Length);
                        tbMessage.Text = newMessage;
                    }
                }

                PinedFiles.Clear();
                
                if (String.IsNullOrWhiteSpace(tbMessage.Text))
                {
                    return;
                }

                byte[] command;
                string message;
                if(cbChooseUser.Text.Trim() == "")
                {
                    message = String.Format(tbMessage.Text);
                    command = new byte[] { 1 };
                }
                else
                {
                    message = String.Format(((UserInfo)cbChooseUser.SelectedItem).ID + tbMessage.Text);
                    command = new byte[] { 2 };
                }
                
                byte[] data = Encoding.Unicode.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);

                byte[] fulldata = length.Concat(command).Concat(data).ToArray();
                socket.Send(fulldata);

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
                    byte[] lengthData = new byte[4];
                    socket.Receive(lengthData);
                    int length = BitConverter.ToInt32(lengthData, 0);
                    byte[] commandData = new byte[1];
                    socket.Receive(commandData);
                    byte command = commandData[0];

                    bytes = socket.Receive(data, length, SocketFlags.None);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

                    string message = builder.ToString();

                    Api api = new Api();

                    ICommand commandRunner = api.GetCommand(command);
                    if (commandRunner != null)
                    {
                        commandRunner.Run(message, this);
                    }
                    else
                    {
                        throw new Exception("Незнакомая команда");
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
        
        public void ChangeUsersList(string[] clients)
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
        public void PrintUsersList()
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
                byte[] command = new byte[] { 3 };
                string message = users.FirstOrDefault(p => p.Name == userName).ID;
                byte[] data = Encoding.Unicode.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] fulldata = length.Concat(command).Concat(data).ToArray();
                socket.Send(fulldata);
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
                byte[] command = new byte[] { 4 };

                // Плохо, потому что ищет свой ID по Имени
                string message = ((UserInfo)cbChooseUser.SelectedItem).ID + users.FirstOrDefault(p => p.Name == userName).ID;
                byte[] data = Encoding.Unicode.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] fulldata = length.Concat(command).Concat(data).ToArray();
                socket.Send(fulldata);

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
        public async void PinFiles(FileDialog pinFileDialog, RichTextBox rtbMessage)
        {
            if (pinFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = pinFileDialog.FileName;

            var client = new HttpClient();
            using (var form = new MultipartFormDataContent())
            {
                using (var fileContent = new ByteArrayContent(File.ReadAllBytes(fileName)))
                {
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(fileContent, "file", Path.GetFileName(fileName));
                    form.Add(new StringContent($"789"), "userId");
                    form.Add(new StringContent("some comments"), "comment");
                    //form.Add(new StringContent("false"), "isPrimary");

                    var stringContent = new StringContent("hello");

                    var response = await client.PostAsync($"{fileServiceUrl}", stringContent); // form
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    //MessageBox.Show(responseContent);
                    var file = JsonConvert.DeserializeObject<FileUploadResult>(responseContent);
                    //MessageBox.Show(file.fileName);

                    rtbMessage.SelectionColor = Color.Blue;
                    rtbMessage.AppendText($"\n{file.fileName}");
                    rtbMessage.SelectionColor = Color.Black;

                    PinedFiles.Add(file);
                }
            }

            //if (pinFileDialog.ShowDialog() == DialogResult.Cancel)
            //    return;
            //string fileName = pinFileDialog.FileName;

            //var client = new HttpClient();
            //using (var form = new MultipartFormDataContent())
            //{
            //    using (var fileContent = new ByteArrayContent(File.ReadAllBytes(fileName)))
            //    {
            //        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            //        form.Add(fileContent, "file", Path.GetFileName(fileName));
            //        form.Add(new StringContent($"789"), "userId");
            //        form.Add(new StringContent("some comments"), "comment");
            //        //form.Add(new StringContent("false"), "isPrimary");

            //        var stringContent = new StringContent("hello");

            //        var response = await client.PostAsync($"{fileServiceUrl}", stringContent); // form
            //        response.EnsureSuccessStatusCode();
            //        var responseContent = await response.Content.ReadAsStringAsync();

            //        //MessageBox.Show(responseContent);
            //        var file = JsonConvert.DeserializeObject<FileUploadResult>(responseContent);
            //        //MessageBox.Show(file.fileName);

            //        rtbMessage.SelectionColor = Color.Blue;
            //        rtbMessage.AppendText($"\n{file.fileName}");
            //        rtbMessage.SelectionColor = Color.Black;

            //        PinedFiles.Add(file);
            //    }
            //}
        }
        public async void SaveFile(SelectedListViewItemCollection items, SaveFileDialog filedialog)
        {
            foreach(var item in items)
            {
                string Id = (string)ReceivedFiles.FirstOrDefault(p => p.LWItem == (ListViewItem)item).LWItem.Tag;
                if (Id !=  null)
                {
                    if (filedialog.ShowDialog() == DialogResult.Cancel)
                        return;
                    string fileName = filedialog.FileName;

                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{fileServiceUrl}/{Id}");
                    response.EnsureSuccessStatusCode();

                    using (var dataStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fileStream = File.Create(fileName))
                        {
                            dataStream.Seek(0, SeekOrigin.Begin);
                            dataStream.CopyTo(fileStream);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Это не файл!");
                }
                
            }
        }
        public async void ShowFileInfo(SelectedListViewItemCollection items)
        {
            foreach (var item in items)
            {
                string Id = (string)ReceivedFiles.FirstOrDefault(p => p.LWItem == (ListViewItem)item).LWItem.Tag;
                if (Id != null)
                {
                    
                    var httpClient = new HttpClient();
                    var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, $"{fileServiceUrl}/{Id}"));
                    response.EnsureSuccessStatusCode();
                    var fileInfo = response.Content.Headers.ToString();
                    MessageBox.Show(fileInfo);
                }
                else
                {
                    MessageBox.Show("Это не файл!");
                }

            }
        }
        public async void DeleteFile(SelectedListViewItemCollection items)
        {
            foreach (var item in items)
            {
                string Id = (string)ReceivedFiles.FirstOrDefault(p => p.LWItem == (ListViewItem)item).LWItem.Tag;
                if (Id != null)
                {
                    if(SentFiles.Any(p => p.Id == Id))
                    {
                        var httpClient = new HttpClient();
                        var response = await httpClient.DeleteAsync($"{fileServiceUrl}/{Id}");
                        response.EnsureSuccessStatusCode();
                        MessageBox.Show("Файл успсешно удален!");
                    }
                    else
                    {
                        MessageBox.Show("Файл был отправлен не вами, вы не можете его удалить!");
                    }
                }
                else
                {
                    MessageBox.Show("Это не файл!");
                }

            }
        }
        public Client(ListView tbChat, Form1 form, ComboBox cbChooseUser)
        {
            Form = form;
            this.tbChat = tbChat;
            this.cbChooseUser = cbChooseUser;
        }
    }
}
