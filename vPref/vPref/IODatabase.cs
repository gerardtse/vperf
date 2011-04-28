using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;

namespace IOStat
{
    class IODatabase
    {
        private static readonly IODatabase instance = new IODatabase();
        private String[] databaseKeys_ = null;
        private SqlConnection conn = null;
        private IODatabase() {
            String connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=\"" + AppDomain.CurrentDomain.BaseDirectory + "IODB.mdf\";Integrated Security=True;User Instance=True";
            Console.WriteLine("New IODatabase Created: " + connectionString);
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("State: {0}", conn.State);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database exception: " + ex.ToString());
            }

            databaseKeys_ = new String[]{ "Disk Reads/sec", "Disk Writes/sec" };
        }

        public static IODatabase getInstance
        {
            get 
            {
                return instance;
            }
        }

        ~IODatabase()
        {
            if (conn != null) conn.Close();
        }

        public SqlConnection getConnection()
        {
            return conn;
        }

        private String componentsByConcatenation(List<String> data, bool quoted=false)
        {
            String buffer = "";
            for (int i = 0; i < data.Count; i++)
            {
                if (quoted) buffer += "'" + data[i] + "'";
                else buffer += data[i].Replace(" ", "_").Replace("/", "_");
                if (i != data.Count - 1) buffer += ",";
            }
            return buffer;
        }

        public bool insertWithDictData(String uuid, Dictionary<String,String> data) {
            if (conn == null)
            {
                Console.WriteLine("Database not available");
                return false;
            }

            List<String> keys = new List<String>();
            List<String> vals = new List<String>();
            keys.Add("agent_id");
            vals.Add(uuid);
            for (int i = 0; i < databaseKeys_.Length; i++ )
            {
                String key = databaseKeys_[i];
                if (!data.ContainsKey(key))
                {
                    Console.WriteLine("Key skipped: " + key);
                    continue;
                }
                keys.Add(key);
                vals.Add(data[key]);
            }
            String insertString = @"insert into Stats ("+ componentsByConcatenation(keys) +") values (" + componentsByConcatenation(vals, true) + ")";
            Console.WriteLine("Data to be inserted: " + insertString);
            SqlCommand cmd = new SqlCommand(insertString, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Unable to insert: " + ex.ToString());
                return false;
            }
            return true;
        }
    }
}
