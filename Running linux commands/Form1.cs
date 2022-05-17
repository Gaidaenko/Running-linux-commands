using Renci.SshNet;
using Running_linux_commands.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Running_linux_commands
{
    public partial class Form1 : Form
    {
        public string userName;
        public string password;
        public string host;
        public int port;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Settings.Default.ServerAddress;
            textBox2.Text = Convert.ToString(Settings.Default.Port);
            textBox3.Text = Settings.Default.Login;
            textBox4.Text = Settings.Default.Password;
        }
        void SaveData()
        {
            Settings.Default.ServerAddress = textBox1.Text;
            Settings.Default.Port = Convert.ToInt32(textBox2.Text);
            Settings.Default.Login = textBox3.Text;
            Settings.Default.Password = textBox4.Text;
            Settings.Default.Save();
        }
        public void RunCommand()
        {
            try
            {
                SshClient sshclient = new SshClient(textBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text, textBox4.Text);
                sshclient.Connect();
                SshCommand command = sshclient.RunCommand("cd /etc/openvpn/easy-rsa/ && source ./vars && ./build-key --batch USER.NAME");
                command.Execute();

                label5.Text = command.Result;
            }
            catch (Exception e)
            {
                label5.Text = "Не удалось установить соединение с сервером!";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunCommand();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}
