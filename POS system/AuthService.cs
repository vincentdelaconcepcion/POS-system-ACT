using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace POS_system
{
    internal sealed class UserAccount
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

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

        public static List<UserAccount> GetUsers()
        {
            EnsureAdmin();

            var users = new List<UserAccount>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                EnsureRoleColumn(con);

                const string sql =
                    "SELECT USERNAME, Role FROM [User] " +
                    "ORDER BY CASE Role WHEN 'Admin' THEN 0 WHEN 'Manager' THEN 1 ELSE 2 END, USERNAME";

                using (var cmd = new SqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserAccount
                        {
                            Username = reader.GetString(0),
                            Role = reader.IsDBNull(1) ? UserRole.Cashier.ToString() : reader.GetString(1)
                        });
                    }
                }
            }

            return users;
        }

        public static void CreateUser(string username, string password, UserRole role)
        {
            EnsureAdmin();
            username = ValidateUsername(username);
            ValidatePassword(password);
            ValidateRole(role);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                EnsureRoleColumn(con);

                using (var check = new SqlCommand("SELECT COUNT(1) FROM [User] WHERE USERNAME = @username", con))
                {
                    check.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;
                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                        throw new InvalidOperationException("A user with that username already exists.");
                }

                using (var cmd = new SqlCommand(
                    "INSERT INTO [User] (USERNAME, PASSWORD, Role) VALUES (@username, @hash, @role)",
                    con))
                {
                    cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@hash", System.Data.SqlDbType.VarChar, 100).Value = HashPassword(password);
                    cmd.Parameters.Add("@role", System.Data.SqlDbType.NVarChar, 20).Value = role.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUserRole(string username, UserRole role)
        {
            EnsureAdmin();
            username = ValidateUsername(username);
            ValidateRole(role);

            if (string.Equals(username, Session.Username, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("You cannot change your own role.");

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                EnsureRoleColumn(con);

                string currentRole = GetRole(con, username);
                if (currentRole == null)
                    throw new InvalidOperationException("The selected user no longer exists.");

                if (string.Equals(currentRole, role.ToString(), StringComparison.OrdinalIgnoreCase))
                    return;

                if (string.Equals(currentRole, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase) &&
                    AdminCount(con, username) == 0)
                {
                    throw new InvalidOperationException("At least one administrator account must remain.");
                }

                using (var cmd = new SqlCommand("UPDATE [User] SET Role = @role WHERE USERNAME = @username", con))
                {
                    cmd.Parameters.Add("@role", System.Data.SqlDbType.NVarChar, 20).Value = role.ToString();
                    cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("The selected user no longer exists.");
                }
            }
        }

        public static void ResetPassword(string username, string password)
        {
            EnsureAdmin();
            username = ValidateUsername(username);
            ValidatePassword(password);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("UPDATE [User] SET PASSWORD = @hash WHERE USERNAME = @username", con))
                {
                    cmd.Parameters.Add("@hash", System.Data.SqlDbType.VarChar, 100).Value = HashPassword(password);
                    cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("The selected user no longer exists.");
                }
            }
        }

        public static void DeleteUser(string username)
        {
            EnsureAdmin();
            username = ValidateUsername(username);

            if (string.Equals(username, Session.Username, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("You cannot delete your own account.");

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                EnsureRoleColumn(con);

                string currentRole = GetRole(con, username);
                if (currentRole == null)
                    throw new InvalidOperationException("The selected user no longer exists.");

                if (string.Equals(currentRole, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase) &&
                    AdminCount(con, username) == 0)
                {
                    throw new InvalidOperationException("At least one administrator account must remain.");
                }

                using (var cmd = new SqlCommand("DELETE FROM [User] WHERE USERNAME = @username", con))
                {
                    cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("The selected user no longer exists.");
                }
            }
        }

        private static void EnsureAdmin()
        {
            if (!Session.CanOpenSettings)
                throw new InvalidOperationException("Only administrators can manage user accounts.");
        }

        private static void EnsureRoleColumn(SqlConnection con)
        {
            if (!ColumnExists(con, "User", "Role"))
                throw new InvalidOperationException(
                    "User roles are not enabled. Run Database POS SYSTEM\\Login-Role-Migration.sql first.");
        }

        private static string ValidateUsername(string username)
        {
            username = (username ?? string.Empty).Trim();

            if (username.Length == 0)
                throw new InvalidOperationException("Username is required.");

            if (username.Length > 50)
                throw new InvalidOperationException("Username cannot be longer than 50 characters.");

            return username;
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new InvalidOperationException("Password is required.");

            if (password.Length < 4)
                throw new InvalidOperationException("Password must be at least 4 characters long.");
        }

        private static void ValidateRole(UserRole role)
        {
            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new InvalidOperationException("Select a valid user role.");
        }

        private static string GetRole(SqlConnection con, string username)
        {
            using (var cmd = new SqlCommand("SELECT Role FROM [User] WHERE USERNAME = @username", con))
            {
                cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? null : Convert.ToString(value);
            }
        }

        private static int AdminCount(SqlConnection con, string excludedUsername)
        {
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM [User] WHERE Role = 'Admin' AND USERNAME <> @username",
                con))
            {
                cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = excludedUsername;
                return Convert.ToInt32(cmd.ExecuteScalar());
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
