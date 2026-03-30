using Npgsql;

namespace SecondPAA___API_With_DB.Helper
{
    public class SqlDBHelper
    {
        private NpgsqlConnection connection;
        private string __constr;

        public SqlDBHelper(string __constr)
        {
          
            this.__constr = __constr;
            connection = new NpgsqlConnection();
            connection.ConnectionString = this.__constr;
        }

        public NpgsqlCommand GetNpgsqlCommand(string query)
        {

            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd;
        }

        public void closeConnection()
        {
            connection.Close();
        }
    }
}
