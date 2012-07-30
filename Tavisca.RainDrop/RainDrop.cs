using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Tavisca.RainDrop
{
    public class RainDrop
    {
        public long GetNextId(long serverId, long workerId, long dataCenterId)
        {
            var gotId = false;
            var retryCount = 0;
            long id = 0;
            while(gotId == false && retryCount < 3 )
            {
                id = GetId();
                if(id == 0)
                {
                    gotId = false;
                    retryCount ++;
                }
                else
                    gotId = true;
            }

            return id;


        }

        private long GetId()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["mysql.seed"].ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                long id = 0;
                using (MySqlCommand command = new MySqlCommand("call spGetNextId()", connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt64("Id");
                    }
                    
                        return id;
                }
                connection.Close();
            }

        }
    }
}
