using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql;

namespace PlantingServer
{
    class database
    {
        public static MySqlConnection getConnection()
        {
            string
                ipAddress = "127.0.0.1",
                user = "root",
                pass = "qkrcksgud1574",
                db = "12dgame";

            int
                port = 3306;

            string mCon = "Server = " + ipAddress + ";Port = " + port + ";Database = " + db + ";Uid = " + user + ";Pwd = " + pass + ";"; //이거 똑바로 안써주면 데이터베이스 접속 안된다.
            return new MySqlConnection(mCon);
        }

        public static bool isDatabaseRead()
        {
            MySqlConnection sqlCon = getConnection();

            try
            {
                sqlCon.Open();

                sqlCon.Close();

            }
            catch (Exception er)
            {
                output.outToScreen("There was an error connecting to the database" + er.ToString());
                sqlCon.Close();
                return false;
            }
            return true;
        }
    }
}
