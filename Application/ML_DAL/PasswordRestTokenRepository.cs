using Microsoft.Data.SqlClient;
using SharedLibraryML;
using SharedLibraryML.Interfaces;
using System.Data;

namespace ML_DAL
{
    public class PasswordResetTokenRepository
        : DatabaseConfiguration, IPasswordResetTokenRepository
    {
        public PasswordResetTokenRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void Create(PasswordResetToken token)
        {
            const string query = @"
                INSERT INTO PasswordResetTokens
                (UserId, TokenHash, ExpiresAt, IsUsed)
                VALUES (@UserId, @TokenHash, @ExpiresAt, 0)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@TokenHash", token.TokenHash);
            cmd.Parameters.AddWithValue("@ExpiresAt", token.ExpiresAt);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateUserPassword(int userId, string hashedPassword)
        {
            const string query = @"
            UPDATE Users
            SET Password = @Password
            WHERE Id = @UserId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        public PasswordResetToken? GetValidToken(string tokenHash)
        {
            const string query = @"
                SELECT Id, UserId, TokenHash, ExpiresAt, IsUsed, CreatedAt
                FROM PasswordResetTokens
                WHERE TokenHash = @TokenHash
                  AND IsUsed = 0
                  AND ExpiresAt > GETUTCDATE()";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@TokenHash", tokenHash);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new PasswordResetToken
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                TokenHash = reader.GetString(2),
                ExpiresAt = reader.GetDateTime(3),
                IsUsed = reader.GetBoolean(4),
                CreatedAt = reader.GetDateTime(5)
            };
        }

        public void MarkAsUsed(string tokenHash)
        {
            const string query = @"
                UPDATE PasswordResetTokens
                SET IsUsed = 1
                WHERE TokenHash = @TokenHash";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@TokenHash", tokenHash);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
