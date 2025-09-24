using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesSystem.Entities;
using System.Data.SqlClient;
using System.Windows.Forms;
using SalesSystem.DTOs.User;

namespace SalesSystem.DAL
{
    internal class UserDAL : BaseDAL
    {
        public UserLoginResponseDTO Login(UserLoginDTO user)
        {
            UserLoginResponseDTO userResponse = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT UserID,FullName, Role 
                                 FROM [User] 
                                 WHERE Email=@Email 
                                 AND PasswordHash=@PasswordHash 
                                 AND Role IN ('Administrator','Seller')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.Password);

                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    userResponse = new UserLoginResponseDTO
                    {
                        ID = (int)reader["UserID"],
                        Name = reader["FullName"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return userResponse;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [User]"; // Verifica que la tabla se llame así
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = (int)reader["UserID"],
                        FullName = reader["FullName"].ToString(),
                        IdentityNumber = (long)reader["IdentityNumber"],
                        Phone = reader["Phone"]?.ToString(),
                        Address = reader["Address"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        RegistrationDate = (DateTime)reader["RegistrationDate"],
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }

            return users;
        }

        public int Delete(DeleteUserDTO userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM [User] WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas;

                }
            }

            catch (Exception ex)
            {
                return 0;
            }
        }


        public void Add(AddUserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [User] 
            (FullName, IdentityNumber, Phone, Address, Email, PasswordHash, Role, RegistrationDate) 
            VALUES (@FullName, @IdentityNumber, @Phone, @Address, @Email, @PasswordHash, @Role, @RegistrationDate)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@IdentityNumber", user.IdentityNumber);
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(user.Phone) ? (object)DBNull.Value : user.Phone);
                cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(user.Address) ? (object)DBNull.Value : user.Address);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                // Si el rol es Customer, guardamos algo simple o vacío en PasswordHash
                string passwordValue = user.Role == "Customer" ? string.Empty : user.PasswordHash;
                cmd.Parameters.AddWithValue("@PasswordHash", passwordValue);

                cmd.Parameters.AddWithValue("@Role", user.Role);

                cmd.Parameters.AddWithValue("@RegistrationDate",
                    user.RegistrationDate == DateTime.MinValue
                        ? DateTime.Now
                        : user.RegistrationDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public User GetById(int userId)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [User] WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = (int)reader["UserID"],
                        FullName = reader["FullName"].ToString(),
                        IdentityNumber = (long)reader["IdentityNumber"],
                        Phone = reader["Phone"]?.ToString(),
                        Address = reader["Address"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        RegistrationDate = (DateTime)reader["RegistrationDate"],
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
            return user;
        }

        public void Update(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [User] 
                         SET FullName=@FullName, IdentityNumber=@IdentityNumber, 
                             Phone=@Phone, Address=@Address, Email=@Email, 
                             PasswordHash=@PasswordHash, Role=@Role
                         WHERE UserID=@UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", user.UserID);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@IdentityNumber", user.IdentityNumber);
                cmd.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<(int UserID, string Display)> GetCustomers()
        {
            var customers = new List<(int, string)>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, FullName, IdentityNumber FROM [User] WHERE Role = 'Customer'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string identity = reader.GetInt64(2).ToString();

                        string display = $"{name} - {identity}";
                        customers.Add((id, display));
                    }
                }
            }

            return customers;
        }


    }
}

