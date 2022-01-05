using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User
    {
        static string conStr = ConfigurationManager.ConnectionStrings["DataAccessContext"].ConnectionString;
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string CONFIRM_PASSWORD { get; set; }
        public static bool Insert(User user) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "INSERT INTO USERS (UserName,Password) VALUES(@USER_NAME,@PASSWORD)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("USER_NAME", user.USER_NAME.ToLower());
                    cmd.Parameters.AddWithValue("PASSWORD", user.PASSWORD.ToLower());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
        public static bool IsAuthenticated(User user) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT UserName,[Password] FROM USERS WHERE UserName = @USER_NAME AND [Password] = @PASSWORD";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    sda.SelectCommand.Parameters.AddWithValue("@USER_NAME", user.USER_NAME);
                    sda.SelectCommand.Parameters.AddWithValue("@PASSWORD", user.PASSWORD);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    DataTable table = ds.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        return user.USER_NAME == dr["UserName"].ToString() && user.PASSWORD == dr["Password"].ToString();
                    }
                }
            }
            catch (Exception ex) 
            {
                
            }
            return false;
        }
    }
}
