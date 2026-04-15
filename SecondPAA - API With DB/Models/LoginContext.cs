using Npgsql;
using SecondPAA___API_With_DB.Helper;
using SecondPAA___API_With_DB.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
namespace SecondPAA___API_With_DB.Models
{
    public class LoginContext
    {
        private string __constr;
        private string __erormsg;

        public LoginContext (string pConstr)
        {
            __constr = pConstr;
        }

        public List<Login> Authentifikasi(string pUsername, string pPassword, IConfiguration pConfig)
        {
            List<Login> list1 = new List<Login> ();
            string query = @"select ps.id_person, ps.nama, ps.alamat, ps.email,
            pp.id_peran, p.nama_peran from person ps
            join peran_person pp using(id_person) 
            join peran p using(id_peran) 
             where ps.username = @username AND ps.password = @pass";
            try
            {
                SqlDBHelper db = new SqlDBHelper(this.__constr);
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@username", pUsername);
                cmd.Parameters.AddWithValue("@pass", pPassword);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list1.Add(new Login()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        id_peran = int.Parse(reader["id_peran"].ToString()),
                        NamaPeran = reader["nama_peran"].ToString(),
                        Token = GenerateJwtToken(pUsername, reader["nama_peran"].ToString(), pConfig)
                    });
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) {
                throw new Exception($"Eror : {ex}");
            }
            return list1;
        }

        private string GenerateJwtToken(string namaUser, string peran, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(pConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, namaUser),
            new Claim(ClaimTypes.Role, peran)
        };

            var token = new JwtSecurityToken(
                pConfig["Jwt:Issuer"],
                pConfig["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

