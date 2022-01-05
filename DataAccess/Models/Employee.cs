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
    public class Employee
    {
        static string conStr = ConfigurationManager.ConnectionStrings["DataAccessContext"].ConnectionString;
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Salary { get; set; }
        public static DataTable GetEmployeesData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT * FROM EMPLOYEES";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetEmployeeDataBasedId(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT * FROM EMPLOYEES WHERE ID = @ID";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    sda.SelectCommand.Parameters.AddWithValue("@ID", Id);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Employee ConvertTableSingleObject(DataTable table)
        {
            try
            {
                Employee employee = new Employee();
                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    employee = new Employee()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Salary = Convert.ToInt32(dr["Salary"])
                    };
                }
                return employee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Employee> ConvertTableToList(DataTable table)
        {
            try
            {
                List<Employee> employees = new List<Employee>();
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow dr = table.Rows[i];
                        Employee employee = new Employee()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            Salary = Convert.ToInt32(dr["Salary"])
                        };
                        employees.Add(employee);
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool Insert(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "INSERT INTO EMPLOYEES (FirstName,LastName,Gender,Salary) VALUES(@FirstName,@LastName,@Gender,@Salary)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
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
        public static bool Update(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "UPDATE EMPLOYEES SET FirstName = @FirstName,LastName = @LastName,Gender = @Gender,Salary = @Salary WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", employee.ID);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
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
        public static bool Delete(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "DELETE FROM EMPLOYEES WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", Id);
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
        public static int InsertEmployeeUsingProcedure(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("AddEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);

                    SqlParameter param = new SqlParameter()
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(param);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(param.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetEmployeesDataUsingProcedure()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("GetEmployeesData", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetEmployeeDataBasedIdUsingProcedure(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("GetEmployeesDataBasedId", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@Id", Id);

                    DataSet ds = new DataSet();

                    sda.Fill(ds);

                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetMaxEmployeeSalaryUsingProcedure()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("maxSalary", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetMaxEmployeeSalaryUsingProcedure_SqlCommand()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("maxSalary", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    int max = Convert.ToInt32(cmd.ExecuteScalar());
                    return max;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
