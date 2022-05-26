using Renci.SshNet;
using Running_linux_commands.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

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

        public void CreateCert()
        {
            label5.Text = null;  

            try
            {
                SshClient sshclient = new SshClient(textBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text, textBox4.Text);
                sshclient.Connect();
                SshCommand command = sshclient.CreateCommand("cd /etc/openvpn/easy-rsa/ && source ./vars && ./build-key --batch " + textBox5.Text);
                command.Execute();
                sshclient.Disconnect();
                
            }
            catch (Exception e)
            {
                label5.Text = "Не удалось установить соединение с сервером!";
            }

            CreateArchiveCert();
        }

        public void CreateArchiveCert()
        {
            label5.Text = null;

            try
            {
                SshClient sshclient = new SshClient(textBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text, textBox4.Text);
                sshclient.Connect();
                SshCommand command = sshclient.CreateCommand("mkdir /tmp/" + textBox5.Text + " && cp /etc/openvpn/easy-rsa/keys/" + textBox5.Text + ".crt " + "/tmp/"
                + textBox5.Text + " && " + "cp /etc/openvpn/easy-rsa/keys/" + textBox5.Text + ".key /tmp/" + textBox5.Text + " && cp /etc/openvpn/easy-rsa/keys/ta.key /tmp/"
                + textBox5.Text + " &&" + " cp /etc/openvpn/easy-rsa/keys/ca.crt /tmp/" + textBox5.Text + " && cd /tmp/" + textBox5.Text + " && tar cvf /tmp/"
                + textBox5.Text + ".tar * && rm -fr /tmp/" + textBox5.Text);               
                label5.Text = command.Execute() + "\n" + "Сертификат и ключ пользователя " + textBox5.Text + " созданы. Архив расположен по пути: /tmp";
                sshclient.Disconnect();
            }
            catch (Exception e)
            {
                label5.Text = "Не удалось установить соединение с сервером!";
            }            
        }

        public void SendMail()
        {
            try
            {
                SshClient sshclient = new SshClient(textBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text, textBox4.Text);
                sshclient.Connect();
                SshCommand command = sshclient.CreateCommand($"echo \"Архив {textBox5.Text} во вложении. \" | mail -s " +
                                                             $"\"Создан сертификат OpenVPN для {textBox5.Text}\" -A /tmp/" + textBox5.Text + ".tar " + textBox6.Text);
                command.Execute();
                sshclient.Disconnect();
            }
            catch
            {
                label5.Text = "Не удалось установить соединение с сервером!";
            }
        }
        public void RevokeCert()
        {
            try
            {
                SshClient sshclient = new SshClient(textBox1.Text, Convert.ToInt32(textBox2.Text), textBox3.Text, textBox4.Text);
                sshclient.Connect();
                SshCommand command = sshclient.CreateCommand("cd /etc/openvpn/easy-rsa/ && source ./vars && ./revoke-full " + textBox5.Text);
                command.Execute();
                sshclient.Disconnect();

                label5.Text = "Сертификат " + textBox5.Text + " отозван!";
            }
            catch
            {
                label5.Text = "Не удалось установить соединение с сервером!";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateCert();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!textBox6.Text.Contains("@") || !textBox6.Text.Contains(""))
            {
                label5.Text = "Строка не содержит електронный адрес!";
                return;
            }

            Thread thired = new Thread(SendMail);
            thired.Start();

            label5.Text = "Сертификат и ключ " + textBox5.Text + " отправлены на почту: " + textBox6.Text + ". Ожидайте!";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            RevokeCert();
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
        private void label5_Click(object sender, EventArgs e)
        {

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
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
