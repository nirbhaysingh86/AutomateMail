using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;
namespace AutomateMail.Services
{
    public static class OracleDatabase
    {
         
        public static string SaveAttachmentData(string data)
        {
            string message = string.Empty;
            //Create connection string. Check whether DBA Privilege is required.
            string  conStringDBA = "TNS_ADMIN=C:\\Users\\kumar\\Oracle\\network\\admin;USER ID=AUTOMAIL;password=automail;DATA SOURCE=DESKTOP-0G7621P:1521/XE";// "User Id=" + sysUser + ";Password=" + sysPwd + ";Data Source=" + db + ";";
            using (OracleConnection con = new OracleConnection(conStringDBA))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        Console.WriteLine("Successfully connected to Oracle Database");
                        Console.WriteLine();
                        //Modify the anonymous PL/SQL GRANT command if you wish to modify the privileges granted
                        cmd.CommandText = "insert into mailresult (quote) values ('"+ data + "')";
                        cmd.ExecuteNonQuery();
                        Console.WriteLine();
                        message = "Ok";
                    }
                    catch (Exception ex)
                    {
                        message = "Fail";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return message;
        }
    }
}

