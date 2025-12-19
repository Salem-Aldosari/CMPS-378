using System;

namespace LostAndFoundSystem.Models
{
    /// <summary>
    /// Represents a system user
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Admin, Staff, Student
        public string Email { get; set; }
        public string ContactInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public User()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        /// <summary>
        /// Check if user has admin privileges
        /// </summary>
        public bool IsAdmin()
        {
            return Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Check if user is staff
        /// </summary>
        public bool IsStaff()
        {
            return Role?.Equals("Staff", StringComparison.OrdinalIgnoreCase) == true ||
                   Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
