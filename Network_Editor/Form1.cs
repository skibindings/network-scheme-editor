using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Editor
{

    public partial class Form1 : Form
    {
        JournalForm journal;
        Workspace workspace;

        int mouse_x, mouse_y;

        public Form1()
        {
            InitializeComponent();

            mouse_x = 0;
            mouse_y = 0;

            ToolStripMenuItem schemeItem = new ToolStripMenuItem("Схема");

            ToolStripMenuItem new_version = new ToolStripMenuItem("Новая версия");
            ToolStripMenuItem save_version = new ToolStripMenuItem("Сохранить версию");
            ToolStripMenuItem load_version = new ToolStripMenuItem("Загрузить актуальную версию");

            new_version.Click += newItem_Click;
            save_version.Click += saveItem_Click;
            load_version.Click += loadItem_Click;

            schemeItem.DropDownItems.Add(new_version);
            schemeItem.DropDownItems.Add(save_version);
            schemeItem.DropDownItems.Add(load_version);

            menuStrip1.Items.Add(schemeItem);

            ToolStripMenuItem journalItem = new ToolStripMenuItem("Журнал");
            journalItem.Click += journalItem_Click;
            menuStrip1.Items.Add(journalItem);

            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += aboutItem_Click;
            menuStrip1.Items.Add(aboutItem);

            panel1.MouseClick += PanelOnMouseClick;
            panel1.MouseMove += PanelOnMouseMove;

            workspace = new Workspace(panel1);
        }

        private void PanelOnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (workspace.start_connection)
                {
                    workspace.first_port = null;
                    panel1.Refresh();
                }
            }
        }

        private void PanelOnMouseMove(object sender, MouseEventArgs e)
        {
            if (workspace.start_connection)
            {
                if(workspace.first_port != null)
                {
                    mouse_x = e.X;
                    mouse_y = e.Y;
                    panel1.Refresh();
                }
            }
        }

        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Название : Редактор схемы сети с поддержкой журналирования\n" +
                "\n"+
                "Автор : ОАО «Russian Software»\n" +
                "\n"+
                "2019 все права защищены");
        }

        void journalItem_Click(object sender, EventArgs e)
        {
            if (!isFormOpened("JournalForm"))
            {
                journal = new JournalForm();
                journal.Show();
            }
            else
            {
                journal.Focus();
            }
        }

        void newItem_Click(object sender, EventArgs e)
        {
            workspace.NewScheme();
        }

        void saveItem_Click(object sender, EventArgs e)
        {
            workspace.SaveToDB();
        }

        void loadItem_Click(object sender, EventArgs e)
        {
            workspace.LoadActualDB();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
            {

                DbConnection conn = db.Database.Connection;
                try
                {
                    conn.Open(); // проверка соединения
                    Console.WriteLine("Connection success");
                }
                catch // ошибка соединения
                {
                    MessageBox.Show("Не удалось подключиться к серверу базы данных");
                    Console.WriteLine("Сonnection failed");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool isFormOpened(string name1)
        {
            bool opened = false;
            FormCollection openforms = Application.OpenForms;

            foreach (Form frms in openforms)
            {
                string name2 = frms.Name;
                if (name2 == name1)
                {
                    opened = true;
                }
            }

            return opened;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            double v_scrollPercentage = (double)
                panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            double h_scrollPercentage = (double)
                panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            workspace.AddDevice(panel1, 0, (int)(h_scrollPercentage * panel1.Width), (int)(v_scrollPercentage * panel1.Height) + 10);

            toolStripButton5.Checked = false;
            workspace.start_connection = false;
            workspace.first_port = null;
            panel1.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            double v_scrollPercentage = (double)
    panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            double h_scrollPercentage = (double)
                panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            workspace.AddDevice(panel1, 1, (int)(h_scrollPercentage * panel1.Width), (int)(v_scrollPercentage * panel1.Height) + 10);

            toolStripButton5.Checked = false;
            workspace.start_connection = false;
            workspace.first_port = null;
            panel1.Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            double v_scrollPercentage = (double)
panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            double h_scrollPercentage = (double)
                panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            workspace.AddDevice(panel1, 2, (int)(h_scrollPercentage * panel1.Width), (int)(v_scrollPercentage * panel1.Height) + 10);

            toolStripButton5.Checked = false;
            workspace.start_connection = false;
            workspace.first_port = null;
            panel1.Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            double v_scrollPercentage = (double)
panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            double h_scrollPercentage = (double)
                panel1.VerticalScroll.Value / panel1.VerticalScroll.Maximum;
            workspace.AddDevice(panel1, 3, (int)(h_scrollPercentage * panel1.Width), (int)(v_scrollPercentage * panel1.Height)+10);

            toolStripButton5.Checked = false;
            workspace.start_connection = false;
            workspace.first_port = null;
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g;

            g = e.Graphics;

            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            for(int i = 0; i < workspace.devices.Count; i++)
            {
                NetDevice dv = workspace.devices[i];
                foreach(NetPort port in dv.ports)
                {
                    if(port.connected_port != null)
                    {
                        g.DrawLine(myPen, port.sprite.Location.X, port.sprite.Location.Y,
                            port.connected_port.sprite.Location.X, port.connected_port.sprite.Location.Y);
                    }
                }
            }

            if (workspace.start_connection)
            {
                if (workspace.first_port != null)
                {
                    g.DrawLine(myPen, workspace.first_port.sprite.Location.X, workspace.first_port.sprite.Location.Y,
                           mouse_x, mouse_y);
                }
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            toolStripButton5.Checked = !toolStripButton5.Checked;
            workspace.start_connection = toolStripButton5.Checked;
            workspace.first_port = null;
        }
    }
}
