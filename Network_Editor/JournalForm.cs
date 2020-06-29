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
    public partial class JournalForm : Form
    {
        Workspace workspace;

        int mouse_x, mouse_y;

        int selected_version_id;

        public JournalForm()
        {
            InitializeComponent();

            mouse_x = 0;
            mouse_y = 0;

            panel1.MouseClick += PanelOnMouseClick;
            panel1.MouseMove += PanelOnMouseMove;

            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;

            workspace = new Workspace(panel1);

            selected_version_id = -1;

            GetVersions();
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
                if (workspace.first_port != null)
                {
                    mouse_x = e.X;
                    mouse_y = e.Y;
                    panel1.Refresh();
                }
            }
        }

        public void GetVersions()
        {
            label2.Text = "Версия: ";
            label4.Text = "Дата: ";
            using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
            {
                List<Network_Versions> db_versions;
                try
                {
                    db_versions = db.Network_Versions.OrderByDescending(p => p.C_Version_Number).ToList();
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки актуальной версии");
                    return;
                }

                List<Control> listControls = new List<Control>();
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    listControls.Add(control);
                }
                foreach (Control control in listControls)
                {
                    flowLayoutPanel1.Controls.Remove(control);
                    control.Dispose();
                }

                for (int i = 0; i < db_versions.Count; i++)
                {
                    Network_Versions db_version = db_versions[i];

                    Button button = new Button();
                    button.Size = new Size(flowLayoutPanel1.Width-6, 30);
                    button.Tag = i;
                    button.BackColor = SystemColors.ButtonFace;
                    button.Text = "Версия " + db_version.C_Version_Number;
                    flowLayoutPanel1.Controls.Add(button);

                    button.Click += delegate
                    {
                        workspace.LoadDB(db_version.ID,db_version.C_Version_Number);

                        label2.Text = "Версия: " + db_version.C_Version_Number;
                        label4.Text = "Дата: " + db_version.C_Date;

                        selected_version_id = db_version.ID;
                    }; 
                }
            }
        }

        private void JournalForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetVersions();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            workspace.NewScheme();
            GetVersions();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (selected_version_id != -1)
            {
                workspace.SaveToDB();
                GetVersions();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected_version_id != -1) {
                using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Network_Connections.RemoveRange(db.Network_Connections.Where(c => c.C_Version == selected_version_id));
                            db.Network_Hardware.RemoveRange(db.Network_Hardware.Where(c => c.C_Version == selected_version_id));
                            db.Network_Settings.RemoveRange(db.Network_Settings.Where(c => c.C_Version == selected_version_id));
                            db.Network_Ports.RemoveRange(db.Network_Ports.Where(c => c.C_Version == selected_version_id));
                            db.Network_Versions.RemoveRange(db.Network_Versions.Where(c => c.ID == selected_version_id));

                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка cохранения версии : " + ex.ToString());
                            transaction.Rollback();
                            return;
                        }
                    }
                }
                workspace.NewScheme();
                GetVersions();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g;

            g = e.Graphics;

            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            for (int i = 0; i < workspace.devices.Count; i++)
            {
                NetDevice dv = workspace.devices[i];
                foreach (NetPort port in dv.ports)
                {
                    if (port.connected_port != null)
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
    }
}
