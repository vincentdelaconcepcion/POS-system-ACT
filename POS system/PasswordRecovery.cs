using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;

namespace POS_system
{
    internal static class PasswordRecovery
    {
        public static string GetRole(string username)
        {
            using (var conn = new SqlConnection(AuthService.ConnectionString))
            using (var cmd = new SqlCommand("SELECT Role FROM [User] WHERE USERNAME = @u", conn))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                conn.Open();
                object r = cmd.ExecuteScalar();
                if (r == null)
                    return null;
                return r == DBNull.Value ? "Cashier" : Convert.ToString(r).Trim();
            }
        }

        public static bool IsAdmin(string role)
        {
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        public static void GetRecoveryInfo(string username, out string question, out bool hasCode)
        {
            question = null;
            hasCode = false;

            using (var conn = new SqlConnection(AuthService.ConnectionString))
            using (var cmd = new SqlCommand(
                "SELECT SecurityQuestion, AnswerHash, RecoveryCodeHash FROM dbo.AdminRecovery WHERE Username = @u", conn))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;
                    if (!r.IsDBNull(0) && !r.IsDBNull(1)) question = r.GetString(0);
                    hasCode = !r.IsDBNull(2);
                }
            }
        }

        public static bool VerifyAnswer(string username, string answer)
        {
            return VerifyColumn(username, "AnswerHash", Normalize(answer));
        }

        public static bool VerifyCode(string username, string code)
        {
            return VerifyColumn(username, "RecoveryCodeHash", Normalize(code));
        }

        private static bool VerifyColumn(string username, string column, string value)
        {
            using (var conn = new SqlConnection(AuthService.ConnectionString))
            using (var cmd = new SqlCommand(
                "SELECT " + column + " FROM dbo.AdminRecovery WHERE Username = @u", conn))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                conn.Open();
                object stored = cmd.ExecuteScalar();
                return stored != null && stored != DBNull.Value && Verify(value, (string)stored);
            }
        }

        public static string SaveRecovery(string username, string question, string answer)
        {
            if (string.IsNullOrWhiteSpace(question) || string.IsNullOrWhiteSpace(answer))
                throw new InvalidOperationException("Question and answer are required.");

            byte[] bytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(bytes);
            string code = BitConverter.ToString(bytes).Replace("-", "");

            using (var conn = new SqlConnection(AuthService.ConnectionString))
            using (var cmd = new SqlCommand(
                @"MERGE dbo.AdminRecovery AS t
                  USING (SELECT @u AS Username) AS s ON t.Username = s.Username
                  WHEN MATCHED THEN UPDATE SET SecurityQuestion=@q, AnswerHash=@a, RecoveryCodeHash=@c
                  WHEN NOT MATCHED THEN INSERT (Username, SecurityQuestion, AnswerHash, RecoveryCodeHash)
                                        VALUES (@u, @q, @a, @c);", conn))
            {
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                cmd.Parameters.Add("@q", SqlDbType.NVarChar, 200).Value = question.Trim();
                cmd.Parameters.Add("@a", SqlDbType.NVarChar, 200).Value = Hash(Normalize(answer));
                cmd.Parameters.Add("@c", SqlDbType.NVarChar, 200).Value = Hash(Normalize(code));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return code;
        }

        public static void SetPassword(string username, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 4)
                throw new InvalidOperationException("Password must be at least 4 characters long.");

            using (var conn = new SqlConnection(AuthService.ConnectionString))
            using (var cmd = new SqlCommand(
                "UPDATE [User] SET PASSWORD = @hash WHERE USERNAME = @u AND Role = 'Admin'", conn))
            {
                cmd.Parameters.Add("@hash", SqlDbType.VarChar, 100).Value = AuthService.HashPassword(newPassword);
                cmd.Parameters.Add("@u", SqlDbType.VarChar, 50).Value = username;
                conn.Open();
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Password was not updated.");
            }
        }

        private static string Normalize(string s)
        {
            return (s ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static string Hash(string text)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
            using (var kdf = new Rfc2898DeriveBytes(text, salt, 100000))
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(kdf.GetBytes(32));
        }

        private static bool Verify(string text, string stored)
        {
            string[] p = (stored ?? string.Empty).Split(':');
            if (p.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(p[0]);
            byte[] expected = Convert.FromBase64String(p[1]);
            using (var kdf = new Rfc2898DeriveBytes(text, salt, 100000))
                return kdf.GetBytes(32).SequenceEqual(expected);
        }
    }
}