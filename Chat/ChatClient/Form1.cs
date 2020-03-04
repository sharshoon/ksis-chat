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

        Client user = new Client();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user.Login(tbName.Text.Trim(), tbIP.Text.Trim(), tbPort.Text.Trim(), tbChat, this);
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
    }
}
