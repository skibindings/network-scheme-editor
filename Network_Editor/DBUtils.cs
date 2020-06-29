using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Editor
{
    class DBUtils
    {
        static string datasource = @"DESKTOP-HVKP5SN\SQLEXPRESS";
        static string database = "Network_Scheme";

        private static string _CONNECTION_STRING_ = "";

        public static string getConnString()
        {
            string server = File.ReadAllText("server.txt", Encoding.UTF8);
            if (_CONNECTION_STRING_.Length == 0)
            {
                //Build an SQL connection string
                SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()
                {
                    DataSource = @server, // Server name
                    InitialCatalog = "Network_Scheme",  //Database
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true,
                    ApplicationName = "EntityFramework",
                };
                //Build an Entity Framework connection string
                EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()
                {
                    Provider = "System.Data.SqlClient",
                    Metadata = "res://*/NetworkModel.csdl|res://*/NetworkModel.ssdl|res://*/NetworkModel.msl",
                    ProviderConnectionString = sqlString.ToString(),
                    
                };
                _CONNECTION_STRING_ = entityString.ConnectionString;
            }
            return _CONNECTION_STRING_;
        }
    }
}
