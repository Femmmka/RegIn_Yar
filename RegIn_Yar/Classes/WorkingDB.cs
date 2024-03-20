using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace RegIn_Yar.Classes
{
    public class WorkingDB
    {
        readonly static string connection ="server=local;port=3306;database=regin;user=root;pwd=";

        public static MySqlConnection OpenConnection()
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(connection);
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
                return null;
            }
        }
        public static MySqlDataReader Quary( string Sql, MySqlConnection mySqlConnection)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(Sql, mySqlConnection);
            return mySqlCommand.ExecuteReader();
        }
        public static void CloseConnection(MySqlConnection mySqlConnection)
        {
            mySqlConnection.Close();
            mySqlConnection.ClearPoolAsync(mySqlConnection);
        }
        public static bool OpenConnection(MySqlConnection mySqlConnection)
        {
            return mySqlConnection != null && mySqlConnection.State == System.Data.ConnectionState.Open;
        }
    }
}
