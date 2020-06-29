using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Editor
{
    public partial class SWSettingsForm : Form
    {
        NetDevice device;
        public SWSettingsForm(NetDevice _device)
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
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            device.workspace.sw_settings = null;
            device.sprite.BackColor = System.Drawing.Color.Transparent;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void SWSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            device.name = textBox1.Text;
            device.ports[0].enabled = radioButton1.Checked;
            device.ports[1].enabled = radioButton4.Checked;
            device.ports[2].enabled = radioButton6.Checked;
            device.ports[3].enabled = radioButton8.Checked;

            device.title.Text = device.name;

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
