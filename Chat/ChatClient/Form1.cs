using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace ChatClient
{
    public partial class Form1 : Form
    {

        Client user;
        ServersFinder broadcast;

        public Form1()
        {
            InitializeComponent();
            user = new Client(lwChat, this, cbChooseUser);
            broadcast = new ServersFinder(this, cbServers);
            btnPinFile.Click += (sender, e) => user.PinFiles(PinFileDialog, rtbMessage);
            tsmiSaveFile.Click += (sender, e) => user.SaveFile(lwChat.SelectedItems, SaveFileDialog);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user.Login(tbName.Text.Trim(), tbIP.Text.Trim(), tbPort.Text.Trim());
        } 
        private void btnSend_Click(object sender, EventArgs e)
        {
            user.SendMessage(rtbMessage);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            user.Disconnect();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            user.Disconnect();
        }

        private void btnFindServer_Click(object sender, EventArgs e)
        {
            broadcast.FindServersRequest();
        }

        private void cbServers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tbIP.Text = ((ServerInfo)cbServers.SelectedItem).Name;
            tbPort.Text = ((ServerInfo)cbServers.SelectedItem).Port;
        }

        private void btnSaveHistory_Click(object sender, EventArgs e)
        {
            //tbChat.Clear();
            lwChat.Clear();
            user.GetDialogHistory();
        }

        private void btnUsersFind_Click(object sender, EventArgs e)
        {
            //tbChat.Clear();
            lwChat.Clear();
            user.GetIndividualDialogHistory();
            // user.SendMessage(tbMessage);
        }

        private void btnMainChat_Click(object sender, EventArgs e)
        {
            cbChooseUser.Text = "";
            //tbChat.Clear();
            lwChat.Clear();
            user.GetDialogHistory();
        }
    }
}
