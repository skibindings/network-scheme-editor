using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Editor
{
    public partial class PCSettingsForm : Form
    {
        NetDevice device;
        public PCSettingsForm(NetDevice _device)
        {
            InitializeComponent();
            this.device = _device;

            textBox1.Text = device.name;
            if(device.ports[0].enabled)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            

            maskedTextBox1.Text = device.ports[0].settings.IP;
            textBox2.Text = device.ports[0].settings.subnet_mask.ToString();
            maskedTextBox2.Text = device.ports[0].settings.default_gateway;

            FormClosing += Form1_Closing;
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            device.workspace.pc_settings = null;
            device.sprite.BackColor = System.Drawing.Color.Transparent;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress;
            if (!IPAddress.TryParse(maskedTextBox1.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге IP!");
                return;
            }
            if (!IPAddress.TryParse(maskedTextBox2.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге шлюза по-умолчанию!");
                return;
            }
            int subnet;
            if (!Int32.TryParse(textBox2.Text, out subnet))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге маски подсети!");
                return;
            }
            if(subnet < 0 || subnet > 32)
            {
                MessageBox.Show("Макска подести должна быть >= 0 и <= 32");
                return;
            }

            device.name = textBox1.Text;
            device.ports[0].enabled = radioButton1.Checked;
            device.ports[0].settings.IP = maskedTextBox1.Text;
            device.ports[0].settings.subnet_mask = subnet;
            device.ports[0].settings.default_gateway = maskedTextBox2.Text;

            device.title.Text = device.name;

            this.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void PCSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
