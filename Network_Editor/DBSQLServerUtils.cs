using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Editor
{
    class DBSQLServerUtils
    {
        public static SqlConnection GetDBConnection(string datasource, string database)
        {
            //Data Source=DESKTOP-HVKP5SN\SQLEXPRESS;Initial Catalog=The_State_Duma_3;User ID=sa;Password=***********
            // Data Source=TRAN-VMWARE\SQLEXPRESS;Initial Catalog=simplehr;Persist Security Info=True;User ID=sa;Password=12345 
            string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;IntegratedSecurity=true";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
    }
}
