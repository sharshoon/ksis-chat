namespace ChatClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbChat = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFindServer = new System.Windows.Forms.Button();
            this.cbServers = new System.Windows.Forms.ComboBox();
            this.btnSaveHistory = new System.Windows.Forms.Button();
            this.cbChooseUser = new System.Windows.Forms.ComboBox();
            this.btnReceiver = new System.Windows.Forms.Button();
            this.btnMainChat = new System.Windows.Forms.Button();
            this.btnPinFile = new System.Windows.Forms.Button();
            this.lwChat = new System.Windows.Forms.ListView();
            this.cmFileInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFileInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.PinFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmFileInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(3, 492);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(401, 53);
            this.tbMessage.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(410, 492);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(142, 53);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbChat
            // 
            this.tbChat.Location = new System.Drawing.Point(649, 142);
            this.tbChat.Multiline = true;
            this.tbChat.Name = "tbChat";
            this.tbChat.Size = new System.Drawing.Size(549, 314);
            this.tbChat.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(410, 98);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(142, 31);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Войти";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(410, 135);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(142, 31);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Выйти";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(55, 102);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(349, 22);
            this.tbName.TabIndex = 5;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(292, 139);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(112, 22);
            this.tbPort.TabIndex = 6;
            this.tbPort.Text = "8888";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(55, 139);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(190, 22);
            this.tbIP.TabIndex = 7;
            this.tbIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Имя:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Port:";
            // 
            // btnFindServer
            // 
            this.btnFindServer.Location = new System.Drawing.Point(13, 12);
            this.btnFindServer.Name = "btnFindServer";
            this.btnFindServer.Size = new System.Drawing.Size(288, 46);
            this.btnFindServer.TabIndex = 11;
            this.btnFindServer.Text = "Найти сервер";
            this.btnFindServer.UseVisualStyleBackColor = true;
            this.btnFindServer.Click += new System.EventHandler(this.btnFindServer_Click);
            // 
            // cbServers
            // 
            this.cbServers.FormattingEnabled = true;
            this.cbServers.Location = new System.Drawing.Point(13, 65);
            this.cbServers.Name = "cbServers";
            this.cbServers.Size = new System.Drawing.Size(288, 24);
            this.cbServers.TabIndex = 12;
            this.cbServers.SelectionChangeCommitted += new System.EventHandler(this.cbServers_SelectionChangeCommitted);
            // 
            // btnSaveHistory
            // 
            this.btnSaveHistory.Location = new System.Drawing.Point(3, 551);
            this.btnSaveHistory.Name = "btnSaveHistory";
            this.btnSaveHistory.Size = new System.Drawing.Size(286, 31);
            this.btnSaveHistory.TabIndex = 13;
            this.btnSaveHistory.Text = "Скачать историю";
            this.btnSaveHistory.UseVisualStyleBackColor = true;
            this.btnSaveHistory.Click += new System.EventHandler(this.btnSaveHistory_Click);
            // 
            // cbChooseUser
            // 
            this.cbChooseUser.FormattingEnabled = true;
            this.cbChooseUser.Location = new System.Drawing.Point(307, 64);
            this.cbChooseUser.Name = "cbChooseUser";
            this.cbChooseUser.Size = new System.Drawing.Size(245, 24);
            this.cbChooseUser.TabIndex = 14;
            // 
            // btnReceiver
            // 
            this.btnReceiver.Location = new System.Drawing.Point(307, 12);
            this.btnReceiver.Name = "btnReceiver";
            this.btnReceiver.Size = new System.Drawing.Size(128, 46);
            this.btnReceiver.TabIndex = 15;
            this.btnReceiver.Text = "Личные сообщения";
            this.btnReceiver.UseVisualStyleBackColor = true;
            this.btnReceiver.Click += new System.EventHandler(this.btnUsersFind_Click);
            // 
            // btnMainChat
            // 
            this.btnMainChat.Location = new System.Drawing.Point(441, 12);
            this.btnMainChat.Name = "btnMainChat";
            this.btnMainChat.Size = new System.Drawing.Size(106, 46);
            this.btnMainChat.TabIndex = 17;
            this.btnMainChat.Text = "Общий чат";
            this.btnMainChat.UseVisualStyleBackColor = true;
            this.btnMainChat.Click += new System.EventHandler(this.btnMainChat_Click);
            // 
            // btnPinFile
            // 
            this.btnPinFile.Location = new System.Drawing.Point(295, 551);
            this.btnPinFile.Name = "btnPinFile";
            this.btnPinFile.Size = new System.Drawing.Size(257, 31);
            this.btnPinFile.TabIndex = 18;
            this.btnPinFile.Text = "Прикрепить файл";
            this.btnPinFile.UseVisualStyleBackColor = true;
            // 
            // lwChat
            // 
            this.lwChat.ContextMenuStrip = this.cmFileInfo;
            this.lwChat.HideSelection = false;
            this.lwChat.Location = new System.Drawing.Point(3, 172);
            this.lwChat.MultiSelect = false;
            this.lwChat.Name = "lwChat";
            this.lwChat.Size = new System.Drawing.Size(549, 314);
            this.lwChat.TabIndex = 19;
            this.lwChat.UseCompatibleStateImageBehavior = false;
            this.lwChat.View = System.Windows.Forms.View.List;
            // 
            // cmFileInfo
            // 
            this.cmFileInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmFileInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveFile,
            this.tsmiFileInfo});
            this.cmFileInfo.Name = "cmFileInfo";
            this.cmFileInfo.Size = new System.Drawing.Size(232, 52);
            // 
            // tsmiSaveFile
            // 
            this.tsmiSaveFile.Name = "tsmiSaveFile";
            this.tsmiSaveFile.Size = new System.Drawing.Size(231, 24);
            this.tsmiSaveFile.Text = "Скачать файл";
            // 
            // tsmiFileInfo
            // 
            this.tsmiFileInfo.Name = "tsmiFileInfo";
            this.tsmiFileInfo.Size = new System.Drawing.Size(231, 24);
            this.tsmiFileInfo.Text = "Информация о файле";
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(3, 492);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(401, 53);
            this.rtbMessage.TabIndex = 20;
            this.rtbMessage.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 591);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.lwChat);
            this.Controls.Add(this.btnPinFile);
            this.Controls.Add(this.btnMainChat);
            this.Controls.Add(this.btnReceiver);
            this.Controls.Add(this.cbChooseUser);
            this.Controls.Add(this.btnSaveHistory);
            this.Controls.Add(this.cbServers);
            this.Controls.Add(this.btnFindServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbChat);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbMessage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.cmFileInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFindServer;
        private System.Windows.Forms.ComboBox cbServers;
        private System.Windows.Forms.Button btnSaveHistory;
        private System.Windows.Forms.ComboBox cbChooseUser;
        private System.Windows.Forms.Button btnReceiver;
        private System.Windows.Forms.Button btnMainChat;
        private System.Windows.Forms.Button btnPinFile;
        private System.Windows.Forms.ListView lwChat;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.OpenFileDialog PinFileDialog;
        private System.Windows.Forms.ContextMenuStrip cmFileInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileInfo;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}

