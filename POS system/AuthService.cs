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
            string role;
            return TryLogin(username, password, out role);
        }

        public static bool TryLogin(string username, string password, out string roleName)
        {
            roleName = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                bool hasRoleColumn = ColumnExists(con, "User", "Role");
                string sql = hasRoleColumn
                    ? "SELECT PASSWORD, Role FROM [User] WHERE USERNAME = @username"
                    : "SELECT PASSWORD FROM [User] WHERE USERNAME = @username";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return false;

                        string expected = ToHex(reader.GetValue(0));
                        if (!FixedTimeEquals(expected, HashPassword(password)))
                            return false;

                        roleName = hasRoleColumn && !reader.IsDBNull(1)
                            ? reader.GetString(1)
                            : null;
                        return true;
                    }
                }
            }
        }

        private static bool ColumnExists(SqlConnection con, string tableName, string columnName)
        {
            const string sql = "SELECT COUNT(1) FROM INFORMATION_SCHEMA.COLUMNS " +
                               "WHERE TABLE_NAME = @table AND COLUMN_NAME = @column";

            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private static string ToHex(object value)
        {
            byte[] raw = value as byte[];
            if (raw != null)
            {
                var sb = new StringBuilder(raw.Length * 2);
                foreach (byte b in raw)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }

            return value == null || value == DBNull.Value ? string.Empty : value.ToString().Trim();
        }

        public static bool UserExists(string username)
        {
            const string sql = "SELECT COUNT(1) FROM [User] WHERE USERNAME = @username";

            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public static void CreateUser(string username, string password, UserRole role)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                bool hasRoleColumn = ColumnExists(con, "User", "Role");
                string sql = hasRoleColumn
                    ? "INSERT INTO [User] (USERNAME, PASSWORD, Role) VALUES (@username, @hash, @role)"
                    : "INSERT INTO [User] (USERNAME, PASSWORD) VALUES (@username, @hash)";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@hash", HashPassword(password));
                    if (hasRoleColumn)
                        cmd.Parameters.AddWithValue("@role", role.ToString());

                    cmd.ExecuteNonQuery();
                }
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
