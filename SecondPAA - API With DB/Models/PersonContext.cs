using Npgsql;
using SecondPAA___API_With_DB.Helper;

namespace SecondPAA___API_With_DB.Models
{
    public class PersonContext
    {
        private string __constr;
        private string __ErrorMsg;

        public PersonContext(string pConstr) { 
         this.__constr = pConstr;
        }

        public List<Person> ListPerson()
        {
            List<Person> listPerson = new List<Person>();
            string query = string.Format(@"Select id_person, nama, alamat, email From person;");
            SqlDBHelper db = new SqlDBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listPerson.Add(new Person()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString()
                    });

                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            }
            return listPerson;
        }
    }
}
