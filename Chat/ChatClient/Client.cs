﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private static TcpClient client;
        private static NetworkStream stream;
        private TextBox tbChat;
        private Form1 Form;

        public void Login(string name, string ip, string port)
        {
            userName = name;
            client = new TcpClient();
            this.host = ip;
            this.port = int.Parse(port);
           
            try
            {
                client.Connect(this.host, this.port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SendMessage(TextBox tbMessage, TextBox tbReceiver)
        {
            try
            {
                string message = tbReceiver.Text.Trim() == "" ? 
                    String.Format(tbMessage.Text) :
                    String.Format(tbMessage.Text + "|" + tbReceiver.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                tbMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ReceiveMessage()
        {
            alive = true;
            while (alive)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

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
                catch
                {
                    MessageBox.Show("Подключение прервано!");
                    Disconnect();
                }
            }
        }
        
        public void Disconnect()
        {
            alive = false;
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            //Environment.Exit(0); //завершение процесса
        }

        public void SaveDialog()
        {
            using(StreamWriter log = new StreamWriter("log.txt", false))
            {
                log.Write(tbChat.Text);
            }
        }
        //public List<string> GetUsers()
        //{
        //    var result = new List<string>();
            
        //}
        public Client(TextBox tbChat, Form1 form)
        {
            Form = form;
            this.tbChat = tbChat;
        }
    }
}
