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

        public Form1()
        {
            InitializeComponent();
            user = new Client(tbChat, this, cbServers);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user.Login(tbName.Text.Trim(), tbIP.Text.Trim(), tbPort.Text.Trim());
        } 
        private void btnSend_Click(object sender, EventArgs e)
        {
            user.SendMessage(tbMessage);
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
            user.BroadCastRequest();
        }

        private void cbServers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tbIP.Text = ((ServerInfo)cbServers.SelectedItem).Name;
            tbPort.Text = ((ServerInfo)cbServers.SelectedItem).Port;
        }

        private void btnSaveHistory_Click(object sender, EventArgs e)
        {
            user.SaveDialog();
        }
    }
}
