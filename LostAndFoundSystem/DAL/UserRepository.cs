using System;
using System.Collections.Generic;
using System.Data.SQLite;
using LostAndFoundSystem.Models;

namespace LostAndFoundSystem.DAL
{
    /// <summary>
    /// Repository for user data access
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Get user by username
        /// </summary>
        public User GetUserByUsername(string username)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND IsActive = 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = reader["Role"].ToString(),
                                Email = reader["Email"]?.ToString() ?? "",
                                ContactInfo = reader["ContactInfo"]?.ToString() ?? "",
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public User GetUserById(int userId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Role = reader["Role"].ToString(),
                                Email = reader["Email"]?.ToString() ?? "",
                                ContactInfo = reader["ContactInfo"]?.ToString() ?? "",
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users ORDER BY Username";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Role = reader["Role"].ToString(),
                            Email = reader["Email"]?.ToString() ?? "",
                            ContactInfo = reader["ContactInfo"]?.ToString() ?? "",
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
                        });
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        public int AddUser(User user)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    INSERT INTO Users (Username, PasswordHash, Role, Email, ContactInfo, IsActive, CreatedDate)
                    VALUES (@Username, @PasswordHash, @Role, @Email, @ContactInfo, @IsActive, @CreatedDate);
                    SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Role", user.Role);
                    command.Parameters.AddWithValue("@Email", user.Email ?? "");
                    command.Parameters.AddWithValue("@ContactInfo", user.ContactInfo ?? "");
                    command.Parameters.AddWithValue("@IsActive", user.IsActive ? 1 : 0);
                    command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Update user role
        /// </summary>
        public void UpdateUserRole(int userId, string role)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Users SET Role = @Role WHERE UserID = @UserID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update user active status
        /// </summary>
        public void UpdateUserStatus(int userId, bool isActive)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Users SET IsActive = @IsActive WHERE UserID = @UserID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
