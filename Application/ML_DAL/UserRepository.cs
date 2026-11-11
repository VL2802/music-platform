using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ML_LogicLayer;
using SharedLibraryML;
using SharedLibraryML.Interfaces;
using System.Data;

namespace ML_DAL
{
    public class UserRepository : DatabaseConfiguration, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(string connectionString, ILogger<UserRepository> logger)
            : base(connectionString)
        {
            _logger = logger;
        }

        public UserRepository(string connectionString)
           : base(connectionString)
        {
            
        }

        #region Create
        public void CreateUser(User user)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_CreateUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Username", user.GetUsername());
                cmd.Parameters.AddWithValue("@Email", user.GetEmail());
                cmd.Parameters.AddWithValue("@PhoneNumber", user.GetPhoneNumber());
                cmd.Parameters.AddWithValue("@Password", user.GetPassword());
                cmd.Parameters.AddWithValue("@Role", (int)user.GetRole());
                cmd.Parameters.AddWithValue("@FileImage", user.GetImage() ?? "");

                conn.Open();
                var insertedId = cmd.ExecuteScalar();
                user.SetId(Convert.ToInt32(insertedId));
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while creating user.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }
        #endregion

        #region Update
        public void UpdateUser(User user)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_UpdateUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", user.GetId());
                cmd.Parameters.AddWithValue("@Username", user.GetUsername());
                cmd.Parameters.AddWithValue("@Email", user.GetEmail());
                cmd.Parameters.AddWithValue("@PhoneNumber", user.GetPhoneNumber());
                cmd.Parameters.AddWithValue("@Password", user.GetPassword());
                cmd.Parameters.AddWithValue("@Role", (int)user.GetRole());
                cmd.Parameters.AddWithValue("@FileImage", user.GetImage());

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while updating user.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }
        #endregion

        #region Delete
        public void DeleteUser(int userId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_DeleteUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while deleting user.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }
        #endregion

        #region Read
        public List<User> ReadAllUsers()
        {
            try
            {
                var users = new List<User>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ReadAllUsers", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Username")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        reader.GetString(reader.GetOrdinal("Password")),
                        (Role)reader.GetInt32(reader.GetOrdinal("Role")),
                        reader.GetString(reader.GetOrdinal("FileImage"))
                    ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while reading users.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reading users");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }

        public User? FindUserByEmail(string email)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_FindUserByEmail", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Username")),
                        reader.GetString(reader.GetOrdinal("Password")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        (Role)reader.GetInt32(reader.GetOrdinal("Role")),
                        reader.GetString(reader.GetOrdinal("FileImage"))
                    );
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while finding user.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding user by email");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }

        public User? ReadUser(int userId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ReadUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(4),
                        reader.GetString(2),
                        reader.GetString(3),
                        (Role)reader.GetInt32(5),
                        reader.GetString(6)
                    );
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(
                    "Database operation failed while reading user.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reading user");
                throw new ApplicationException(
                    "An unexpected error occurred. Please try again later.");
            }
        }
        #endregion
    }
}
