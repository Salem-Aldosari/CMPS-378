using System;
using LostAndFoundSystem.Models;
using LostAndFoundSystem.DAL;

namespace LostAndFoundSystem.BLL
{
    /// <summary>
    /// Service for authentication and authorization
    /// </summary>
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private static User _currentUser;

        public AuthService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Get the currently logged-in user
        /// </summary>
        public static User CurrentUser
        {
            get { return _currentUser; }
        }

        /// <summary>
        /// Check if a user is currently logged in
        /// </summary>
        public static bool IsLoggedIn
        {
            get { return _currentUser != null; }
        }

        /// <summary>
        /// Authenticate user with username and password
        /// </summary>
        public bool Login(string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "Username is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password is required.";
                return false;
            }

            User user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                errorMessage = "Invalid username or password.";
                return false;
            }

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                errorMessage = "Invalid username or password.";
                return false;
            }

            _currentUser = user;
            return true;
        }

        /// <summary>
        /// Log out the current user
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public bool Register(string username, string password, string role, string email, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "Username is required.";
                return false;
            }

            if (username.Length < 3)
            {
                errorMessage = "Username must be at least 3 characters.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password is required.";
                return false;
            }

            if (password.Length < 6)
            {
                errorMessage = "Password must be at least 6 characters.";
                return false;
            }

            // Check if username already exists
            User existingUser = _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                errorMessage = "Username already exists.";
                return false;
            }

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role,
                Email = email
            };

            _userRepository.AddUser(newUser);
            return true;
        }

        /// <summary>
        /// Check if current user has admin privileges
        /// </summary>
        public static bool IsAdmin()
        {
            return _currentUser != null && _currentUser.IsAdmin();
        }

        /// <summary>
        /// Check if current user is staff (includes admin)
        /// </summary>
        public static bool IsStaff()
        {
            return _currentUser != null && _currentUser.IsStaff();
        }

        /// <summary>
        /// Get current user's role
        /// </summary>
        public static string GetCurrentUserRole()
        {
            return _currentUser?.Role ?? "Guest";
        }
    }
}
