using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static POSServices.Models.HO_MsgModel;

namespace POSServices.WebAPIPOSMsgController
{
    [Route("homsg/ftpserver")]
    [ApiController]
    public class FTPServerController : Controller
    {
        [HttpGet]
        public List<ftpServer> GetFtpServers()
        {
            List<ftpServer> ftpServersList = new List<ftpServer>();
            try
            {
                string ConnectionString = getConnection();

                SqlConnection con = new SqlConnection(ConnectionString);

                SqlConnection connection = con;
                try
                {

                    string queryString = "SELECT * FROM FTPServerTable";                    

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {                            
                            while (reader.Read())
                            {
                                ftpServer temp = new ftpServer();
                                temp.serverName = Convert.ToString(reader["ServerName"]);
                                temp.userName = Convert.ToString(reader["Username"]);
                                temp.password = Convert.ToString(reader["Password"]);

                                ftpServersList.Add(temp);
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                return ftpServersList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string getConnection()
        {
            return Startup.POSMsgConnString;
        }
    }
}