using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace POS_system
{
    internal static class AuthService
    {
        private static string ConnectionString
        {
            get
            {
                var setting = System.Configuration.ConfigurationManager.ConnectionStrings["LogInConnectionString"];
                if (setting == null || string.IsNullOrEmpty(setting.ConnectionString))
                    throw new InvalidOperationException("LogInConnectionString is missing from App.config");
                return setting.ConnectionString;
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            const string sql = "SELECT PasswordHash FROM Users WHERE Username = @username";

            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();

                object stored = cmd.ExecuteScalar();
                if (stored == null || stored == DBNull.Value)
                    return false;

                string expected;
                byte[] raw = stored as byte[];
                if (raw != null)
                {
                    var sb = new StringBuilder(raw.Length * 2);
                    foreach (byte b in raw)
                        sb.Append(b.ToString("x2"));
                    expected = sb.ToString();
                }
                else
                {
                    expected = stored.ToString().Trim();
                }

                return FixedTimeEquals(expected, HashPassword(password));
            }
        }

        public static bool UserExists(string username)
        {
            const string sql = "SELECT COUNT(1) FROM Users WHERE Username = @username";

            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public static void CreateUser(string username, string password)
        {
            const string sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@username, @hash)";

            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@hash", HashPassword(password));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static bool FixedTimeEquals(string expected, string actual)
        {
            if (expected.Length != actual.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < expected.Length; i++)
                diff |= char.ToUpperInvariant(expected[i]) ^ char.ToUpperInvariant(actual[i]);
            return diff == 0;
        }
    }
}
