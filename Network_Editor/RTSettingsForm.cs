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
    public partial class RTSettingsForm : Form
    {
        NetDevice device;
        public RTSettingsForm(NetDevice _device)
        {
            InitializeComponent();
            this.device = _device;

            textBox1.Text = device.name;
            if (device.ports[0].enabled)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            if (device.ports[1].enabled)
            {
                radioButton4.Checked = true;
            }
            else
            {
                radioButton3.Checked = true;
            }
            if (device.ports[2].enabled)
            {
                radioButton6.Checked = true;
            }
            else
            {
                radioButton5.Checked = true;
            }
            if (device.ports[3].enabled)
            {
                radioButton8.Checked = true;
            }
            else
            {
                radioButton7.Checked = true;
            }

            FormClosing += Form1_Closing;

            // 1 4 6 8
            maskedTextBox1.Text = device.ports[0].settings.IP;
            maskedTextBox4.Text = device.ports[1].settings.IP;
            maskedTextBox6.Text = device.ports[2].settings.IP;
            maskedTextBox8.Text = device.ports[3].settings.IP;

            textBox2.Text = device.ports[0].settings.subnet_mask.ToString();
            textBox3.Text = device.ports[1].settings.subnet_mask.ToString();
            textBox4.Text = device.ports[2].settings.subnet_mask.ToString();
            textBox5.Text = device.ports[3].settings.subnet_mask.ToString();

            // там взрослая тетя сидит справа
            // и чё
           

        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            device.workspace.rt_settings = null;
            device.sprite.BackColor = System.Drawing.Color.Transparent;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RTSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox6_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox8_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress;
            if (!IPAddress.TryParse(maskedTextBox1.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге IP порта 1!");
                return;
            }
            if (!IPAddress.TryParse(maskedTextBox4.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге IP порта 2!");
                return;
            }
            if (!IPAddress.TryParse(maskedTextBox6.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге IP порта 3!");
                return;
            }
            if (!IPAddress.TryParse(maskedTextBox8.Text, out ipAddress))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге IP порта 4!");
                return;
            }



            int subnet_1;
            if (!Int32.TryParse(textBox2.Text, out subnet_1))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге маски подсети порта 1!");
                return;
            }
            if (subnet_1 < 0 || subnet_1 > 32)
            {
                MessageBox.Show("Макска подести должна быть >= 0 и <= 32 (порт 1)");
                return;
            }

            int subnet_2;
            if (!Int32.TryParse(textBox3.Text, out subnet_2))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге маски подсети порта 2!");
                return;
            }
            if (subnet_2 < 0 || subnet_2 > 32)
            {
                MessageBox.Show("Макска подести должна быть >= 0 и <= 32 (порт 2)");
                return;
            }

            int subnet_3;
            if (!Int32.TryParse(textBox4.Text, out subnet_3))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге маски подсети порта 3!");
                return;
            }
            if (subnet_3 < 0 || subnet_3 > 32)
            {
                MessageBox.Show("Макска подести должна быть >= 0 и <= 32 (порт 3)");
                return;
            }

            int subnet_4;
            if (!Int32.TryParse(textBox5.Text, out subnet_4))
            {
                //is not valid ip
                MessageBox.Show("Ошибка при парсинге маски подсети порта 4!");
                return;
            }
            if (subnet_4 < 0 || subnet_4 > 32)
            {
                MessageBox.Show("Макска подести должна быть >= 0 и <= 32 (порт 4)");
                return;
            }


            device.name = textBox1.Text;
            device.ports[0].enabled = radioButton1.Checked;
            device.ports[1].enabled = radioButton4.Checked;
            device.ports[2].enabled = radioButton6.Checked;
            device.ports[3].enabled = radioButton8.Checked;

            device.title.Text = device.name;

            device.ports[0].settings.subnet_mask = subnet_1;
            device.ports[1].settings.subnet_mask = subnet_2;
            device.ports[2].settings.subnet_mask = subnet_3;
            device.ports[3].settings.subnet_mask = subnet_4;

            device.ports[0].settings.IP = maskedTextBox1.Text;
            device.ports[1].settings.IP = maskedTextBox4.Text;
            device.ports[2].settings.IP = maskedTextBox6.Text;
            device.ports[3].settings.IP = maskedTextBox8.Text;

            this.Close();
        }
    }
}
