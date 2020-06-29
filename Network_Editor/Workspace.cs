using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Editor
{
    public class NetSettings
    {
        public static int id = 0;
        public int id_real;

        public String IP;
        public int subnet_mask;
        public String default_gateway;

        public NetSettings()
        {
            id_real = id;
            id++;

            IP = "192.168.0.1";
            subnet_mask = 24;
            default_gateway = "224.0.0.0";
        }
    }

    public class NetPort
    {
        public static int id = 0;
        public int id_real;

        public int settings_id;

        public NetSettings settings;
        public bool enabled;
        public PictureBox sprite;

        public NetPort connected_port;

        public NetDevice device;

        ContextMenuStrip context;

        public NetPort(NetDevice new_device)
        {
            id_real = id;
            id++;

            device = new_device;
            enabled = false;
            settings = new NetSettings();

            sprite = new PictureBox
            {
                Name = "port",
                BackColor = System.Drawing.Color.Transparent,
                Size = new Size(32, 32),
                Location = new Point(5, 5),
                Visible = true,
                Image = Image.FromFile("port.png"),
            };
            sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            sprite.Size = new Size(6, 6);
            sprite.MouseClick += OnMouseClick;

            connected_port = null;

            context = new ContextMenuStrip();
            // создаем элементы меню
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить связь");
            // добавляем элементы в меню
            context.Items.Add(deleteMenuItem);
            // ассоциируем контекстное меню с текстовым полем
            sprite.ContextMenuStrip = context;
            // устанавливаем обработчики событий для меню
            deleteMenuItem.Click += deleteMenuItem_Click;

        }

        public void setPositionAndUpdate(int x, int y)
        {
            sprite.Location = new Point(x, y);
        }

        public void setVisible(bool visible)
        {
            sprite.Visible = visible;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (device.workspace.start_connection)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (device.workspace.first_port == null)
                    {
                        if (connected_port != null) return;
                        device.workspace.first_port = this;
                    }
                    else
                    {
                        if (connected_port != null) return;
                        foreach (NetPort port in device.ports)
                        {
                            if (port == device.workspace.first_port)
                                return;
                        }
                        device.workspace.first_port.connected_port = this;
                        this.connected_port = device.workspace.first_port;

                        device.workspace.first_port = null;
                        device.workspace.panel.Refresh();
                    }
                }
            }
            else
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                }
            }
        }

        void deleteMenuItem_Click(object sender, EventArgs e)
        {
            //if (device.workspace.start_connection)
            //{
                if (device.workspace.first_port == null)
                {
                    deleteConnection();
                }
            //}
        }

        public void deleteConnection()
        {
            if (connected_port != null)
            {
                connected_port.connected_port = null;
                connected_port = null;
                device.workspace.panel.Refresh();
            }
        }
    }


    public class Workspace
    {
        public NetDevice touched_device;

        public List<NetDevice> devices;

        public PCSettingsForm pc_settings;
        public SWSettingsForm sw_settings;
        public RTSettingsForm rt_settings;
        public PRSettingsForm pr_settings;

        public bool start_connection;

        public NetPort first_port;

        public Panel panel;

        int version;
        int version_id;

        int starting_id;

        public Workspace(Panel p)
        {
            devices = new List<NetDevice>();

            panel = p;

            touched_device = null;

            pc_settings = null;
            sw_settings = null;
            rt_settings = null;
            pr_settings = null;

            start_connection = false;
            first_port = null;

            GetActualVersion();
        }

        public void AddDevice(Panel p, int type, int x, int y)
        {
            NetDevice new_device = new NetDevice(this, type, true);
            new_device.x = x;
            new_device.y = y;
            new_device.update();
            new_device.drawToForm(p);
            devices.Add(new_device);
        }

        public void RefreshDraw()
        {
            panel.Controls.Clear();
            foreach(NetDevice device in devices)
            {
                device.update();
                device.drawToForm(panel);
            }
        }

        public void GetActualVersion()
        {
            using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
            {
                try
                {
                    if (db.Network_Versions.Count() == 0)
                    {
                        version = 0;
                        version_id = 0;
                        starting_id = (version_id + 1) * 1000;
                    }
                    else
                    {
                        Network_Versions version_obj = db.Network_Versions.FirstOrDefault
                            (p => p.ID == db.Network_Versions.Max(x => x.ID));
                        version = version_obj.C_Version_Number;
                        version_id = version_obj.ID;
                        starting_id = (version_id+1) * 1000;
                    } 
                }
                catch
                {
                    MessageBox.Show("Ошибка получения актуальной версии");
                }
            }
        }


        public void NewScheme()
        {
            panel.Controls.Clear();
            devices.Clear();

            NetDevice.id = 0;
            NetSettings.id = 0;
            NetPort.id = 0;
            NetDevice.pc_id = 0;
            NetDevice.pr_id = 0;
            NetDevice.sw_id = 0;
            NetDevice.ro_id = 0;

            panel.Refresh();
        }

        public void LoadDB(int load_version_id, int version_number)
        {
            GetActualVersion();
            using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
            {
                List<Network_Hardware> db_devices;
                List<Network_Settings> db_settings;
                List<Network_Ports> db_ports;
                List<Network_Connections> db_connections;
                try
                {
                    db_devices = db.Network_Hardware.Where(b => b.C_Version == load_version_id).ToList<Network_Hardware>();
                    db_settings = db.Network_Settings.Where(b => b.C_Version == load_version_id).ToList<Network_Settings>();
                    db_ports = db.Network_Ports.Where(b => b.C_Version == load_version_id).ToList<Network_Ports>();
                    db_connections = db.Network_Connections.Where(b => b.C_Version == load_version_id).ToList<Network_Connections>();
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки актуальной версии");
                    return;
                }
                panel.Controls.Clear();
                devices.Clear();

                NetDevice.id = 0;
                NetSettings.id = 0;
                NetPort.id = 0;
                NetDevice.pc_id = 0;
                NetDevice.pr_id = 0;
                NetDevice.sw_id = 0;
                NetDevice.ro_id = 0;

                int max_real_id = 0;
                int id_shift = (load_version_id) * (-1000);

                foreach (Network_Hardware db_device in db_devices)
                {
                    int id = id_shift + db_device.ID;
                    if (id > max_real_id)
                    {
                        max_real_id = id;
                    }
                    NetDevice device = new NetDevice(this, db_device.C_Type, false);
                    device.x = db_device.C_Interface_X;
                    device.y = db_device.C_Interface_Y;
                    device.name = db_device.C_Name;
                    device.title.Text = device.name;
                    device.id_real = id;

                    devices.Add(device);
                }
                NetDevice.id = max_real_id + 1;

                max_real_id = 0;
                foreach (Network_Ports db_port in db_ports)
                {
                    int id = id_shift + db_port.ID;
                    if (id > max_real_id)
                    {
                        max_real_id = id;
                    }
                    NetPort port = new NetPort(null);
                    port.id_real = id;
                    port.enabled = db_port.C_Status;
                    port.settings_id = id_shift + db_port.C_Settings_ID;

                    // set device
                    int device_id = id_shift + db_port.C_Hardware_ID;
                    foreach (NetDevice device in devices)
                    {
                        if (device.id_real == device_id)
                        {
                            port.device = device;
                            for (int i = 0; i < device.ports.Count; i++)
                            {
                                if (device.ports[i] == null)
                                {
                                    device.ports[i] = port;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                NetPort.id = max_real_id + 1;

                max_real_id = 0;
                foreach (Network_Settings db_setting in db_settings)
                {
                    int id = id_shift + db_setting.ID;
                    if (id > max_real_id)
                    {
                        max_real_id = id;
                    }
                    NetSettings setting = new NetSettings();
                    setting.id_real = id;
                    setting.IP = db_setting.C_IP;
                    setting.default_gateway = db_setting.C_Default_Gateway;
                    setting.subnet_mask = db_setting.C_Subnet_Mask;

                    // set device
                    bool stop = false;
                    foreach (NetDevice device in devices)
                    {
                        if (stop)
                            break;
                        foreach (NetPort port in device.ports)
                        {
                            if (setting.id_real == port.settings_id)
                            {
                                port.settings = setting;
                                stop = true;
                                break;
                            }
                        }
                    }
                }
                NetSettings.id = max_real_id + 1;

                foreach (Network_Connections db_connection in db_connections)
                {
                    int port_start_id = id_shift + db_connection.C_From_Port_ID;
                    int port_end_id = id_shift + db_connection.C_To_Port_ID;

                    NetPort start_port = null;
                    NetPort end_port = null;

                    bool stop = false;
                    foreach (NetDevice device in devices)
                    {
                        if (stop)
                            break;
                        foreach (NetPort port in device.ports)
                        {
                            if (port.id_real == port_start_id)
                            {
                                start_port = port;
                                stop = true;
                                break;
                            }
                        }
                    }

                    stop = false;
                    foreach (NetDevice device in devices)
                    {
                        if (stop)
                            break;
                        foreach (NetPort port in device.ports)
                        {
                            if (port.id_real == port_end_id)
                            {
                                end_port = port;
                                stop = true;
                                break;
                            }
                        }
                    }

                    start_port.connected_port = end_port;
                    end_port.connected_port = start_port;
                }
            }
            RefreshDraw();
            panel.Refresh();
            MessageBox.Show("Успешно загружена версия : " + version_number);
        }


        public void LoadActualDB()
        {
            if(version == 0)
            {
                MessageBox.Show("База данных не содержит ни одной версии схемы сети!");
                return;
            }
            LoadDB(version_id, version);
        }

        public void SaveToDB()
        {
            
            using (Network_SchemeEntities db = new Network_SchemeEntities(DBUtils.getConnString()))
            {
                Network_Versions new_version = new Network_Versions();
                new_version.C_Date = DateTime.Now.ToString("d/M/yyyy");
                new_version.C_Version_Number = version + 1;
                new_version.ID = version_id + 1;

                List<Network_Hardware> db_devices = new List<Network_Hardware>();
                List<Network_Settings> db_settings = new List<Network_Settings>();
                List<Network_Ports> db_ports = new List<Network_Ports>();
                List<Network_Connections> db_connections = new List<Network_Connections>();

                int connection_id = 0;
                foreach (NetDevice device in devices)
                {
                    Network_Hardware db_device = new Network_Hardware();
                    db_device.ID = starting_id + device.id_real;
                    db_device.C_Interface_X = device.sprite.Location.X;
                    db_device.C_Interface_Y = device.sprite.Location.Y;
                    db_device.C_Name = device.name;
                    db_device.C_Type = device.type;
                    db_device.C_Status = false;
                    db_device.C_Version = new_version.ID;

                    db_devices.Add(db_device);

                    foreach(NetPort port in device.ports)
                    {
                        Network_Settings db_setting = new Network_Settings();
                        db_setting.ID = starting_id + port.settings.id_real;
                        db_setting.C_IP = port.settings.IP;
                        db_setting.C_Subnet_Mask = port.settings.subnet_mask;
                        db_setting.C_Default_Gateway = port.settings.default_gateway;
                        db_setting.C_Version = new_version.ID;

                        db_settings.Add(db_setting);

                        Network_Ports db_port = new Network_Ports();
                        db_port.ID = starting_id + port.id_real;
                        db_port.C_Status = port.enabled;
                        db_port.C_Version = new_version.ID;
                        db_port.C_Hardware_ID = db_device.ID;
                        db_port.C_Settings_ID = db_setting.ID;

                        db_ports.Add(db_port);

                        if(port.connected_port != null)
                        {
                            Network_Connections db_connection = new Network_Connections();
                            db_connection.ID = starting_id + connection_id;
                            db_connection.C_Version = new_version.ID;
                            db_connection.C_From_Port_ID = starting_id + port.id_real;
                            db_connection.C_To_Port_ID = starting_id + port.connected_port.id_real;
                            db_connections.Add(db_connection);

                            connection_id++;
                        }
                    }
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Network_Versions.Add(new_version);

                        foreach(Network_Settings db_setting in db_settings)
                        {
                            db.Network_Settings.Add(db_setting);
                        }
                        foreach (Network_Hardware db_device in db_devices)
                        {
                            db.Network_Hardware.Add(db_device);
                        }
                        foreach (Network_Ports db_port in db_ports)
                        {
                            db.Network_Ports.Add(db_port);
                        }
                        foreach (Network_Connections db_connection in db_connections)
                        {
                            db.Network_Connections.Add(db_connection);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка cохранения версии : " + ex.ToString());
                        transaction.Rollback();
                    }
                }
            }
            MessageBox.Show("Успешно сохранена версия : " + (version+1));
            GetActualVersion();
        }
    }

    public class NetDevice
    {
        public static int id = 0;
        public static int pc_id;
        public static int sw_id;
        public static int ro_id;
        public static int pr_id;

        public int x, y;
        public String name;
        public int type;

        public List<NetPort> ports;

        public PictureBox sprite;
        public Label title;
        public Workspace workspace;

        private Point MouseDownLocation;

        ContextMenuStrip context;

        public int id_real;

        public NetDevice(Workspace new_workspace, int type, bool init_port)
        {
            id_real = id;
            id++;

            x = 800;
            y = 100;
            name = "0" + id;
            this.type = type;

            workspace = new_workspace;

            ports = new List<NetPort>();

            sprite = new PictureBox
            {
                Name = "spr" + id,
                BackColor = System.Drawing.Color.Transparent,
                Size = new Size(32, 32),
                Location = new Point(20, 20),
                Visible = true
            };
            sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            sprite.MouseDoubleClick += MouseDoubleClick_Event;
            sprite.MouseDown += OnMouseDown;
            sprite.MouseMove += OnMouseMove;

            switch (type)
            {
                case 0: // PC               
                    name = "PC" + pc_id;
                    pc_id++;
                    sprite.Image = Image.FromFile("laptop.png");

                    for(int i = 0; i < 1; i++)
                    {
                        if (init_port)
                        {
                            ports.Add(new NetPort(this));
                        }
                        else
                        {
                            ports.Add(null);
                        }
                    }

                    break;
                case 1: // SW               
                    name = "SW" + sw_id;
                    sw_id++;
                    sprite.Image = Image.FromFile("switch.png");
                    sprite.Size = new Size(50, 32);

                    for (int i = 0; i < 4; i++)
                    {
                        if (init_port)
                        {
                            ports.Add(new NetPort(this));
                        }
                        else
                        {
                            ports.Add(null);
                        }
                    }

                    break;
                case 2: // RO               
                    name = "RT" + ro_id;
                    ro_id++;
                    sprite.Image = Image.FromFile("router.png");

                    for (int i = 0; i < 4; i++)
                    {
                        if (init_port)
                        {
                            ports.Add(new NetPort(this));
                        }
                        else
                        {
                            ports.Add(null);
                        }
                    }

                    break;
                case 3: // PR               
                    name = "PR" + pr_id;
                    pr_id++;
                    sprite.Image = Image.FromFile("provider.png");

                    for (int i = 0; i < 1; i++)
                    {
                        if (init_port)
                        {
                            ports.Add(new NetPort(this));
                        }
                        else
                        {
                            ports.Add(null);
                        }
                    }

                    break;
            }

            title = new Label();
            title.AutoSize = true;
            title.Text = name;

            context = new ContextMenuStrip();
            // создаем элементы меню
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить элемент");
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("Настроить");
            // добавляем элементы в меню
            context.Items.Add(settingsMenuItem);
            context.Items.Add(deleteMenuItem);
            // ассоциируем контекстное меню с текстовым полем
            sprite.ContextMenuStrip = context;
            // устанавливаем обработчики событий для меню
            deleteMenuItem.Click += deleteMenuItem_Click;
            settingsMenuItem.Click += settingsMenuItem_Click;
        }

        public void update()
        {
            sprite.Location = new Point(x, y);
            title.Location = new Point(x+4, y-12);

            switch (type)
            {
                case 0: // PC  
                    ports[0].setPositionAndUpdate(x+12, y + 32);
                    break;
                case 1: // SW  
                    int i = 0;
                    foreach (NetPort port in ports)
                    {

                        port.setPositionAndUpdate(x+i*12, y + 32);
                        i++;
                    }
                    break;
                case 2: // RT  
                    ports[2].setPositionAndUpdate(x + 2, y + 32);
                    ports[1].setPositionAndUpdate(x + 22, y + 32);
                    ports[0].setPositionAndUpdate(x - 12, y + 10);
                    ports[3].setPositionAndUpdate(x + 39, y + 10);
                    break;
                case 3: // PR  
                    ports[0].setPositionAndUpdate(x + 12, y + 36);
                    break;
            }
            
        }

        public void drawToForm(Panel f)
        {
            foreach (NetPort port in ports)
            {
                f.Controls.Add(port.sprite);
                port.sprite.Show();
                port.sprite.BringToFront();
            }

            f.Controls.Add(sprite);
            sprite.Show();
            sprite.BringToFront();
            f.Controls.Add(title);
            title.Show();
            title.BringToFront();
        }

        public void openSettings()
        {
            if (workspace.pc_settings == null &&
            workspace.sw_settings == null &&
            workspace.rt_settings == null &&
            workspace.pr_settings == null)
            {
                sprite.BackColor = System.Drawing.Color.LimeGreen;
                switch (type)
                {
                    case 0: // PC    

                        workspace.pc_settings = new PCSettingsForm(this);
                        workspace.pc_settings.Show();

                        break;
                    case 1: // SW        

                        workspace.sw_settings = new SWSettingsForm(this);
                        workspace.sw_settings.Show();

                        break;
                    case 2: // RO 

                        workspace.rt_settings = new RTSettingsForm(this);
                        workspace.rt_settings.Show();

                        break;
                    case 3: // PR        

                        workspace.pr_settings = new PRSettingsForm(this);
                        workspace.pr_settings.Show();

                        break;
                }
            }
        }

        private void MouseDoubleClick_Event(object sender, MouseEventArgs e)
        {
            if (workspace.start_connection)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                openSettings();
            }
        }


        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (workspace.start_connection)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (workspace.start_connection)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                x = e.X + sprite.Left - MouseDownLocation.X;
                y = e.Y + sprite.Top - MouseDownLocation.Y;

                update();
                workspace.panel.Refresh();
            }
        }

        void deleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach(NetPort port in ports)
            {
                port.deleteConnection();
            }
            // find this device index in array
            int delete_id = -1;
            for(int i = 0; i < workspace.devices.Count(); i++)
            {
                NetDevice d = workspace.devices[i];
                if(d.name.Equals(this.name))
                {
                    delete_id = i;
                }
            }
            if(delete_id != -1)
            {
                workspace.devices.RemoveAt(delete_id);
                workspace.RefreshDraw();
            }
        }

        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            openSettings();
        }
    }
}
