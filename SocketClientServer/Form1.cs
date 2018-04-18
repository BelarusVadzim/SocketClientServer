using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketClientServer.Controllers;

namespace SocketClientServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SocketServer SS;
        bool listen = false;

        private void button1_Click(object sender, EventArgs e)
        {
            WriteToLog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("rrr");
        }

        private async void WriteToLog()
        {
            label1.Text = "Server is started";
            listen = true;
            SS = new SocketServer();
            SS.InitServer();
            SS.StartListen();
            StringBuilder SB = new StringBuilder();
            while (listen)
            {
                SB.Append(DateTime.Now.ToString());
                SB.Append(" ");
                SB.Append(await SS.WaitForMessage());
                SB.AppendLine();
                richTextBox1.Text += SB.ToString();
                SB.Clear();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listen = false;
            SS.StopListen();
            label1.Text = "Server is stoped";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SocketClient SC = new SocketClient();
            SC.SendData(textBox1.Text);
        }
    }
}
