using System;
using System.Windows.Forms;
using LostAndFoundSystem.DAL;
using LostAndFoundSystem.UI;

namespace LostAndFoundSystem
{
    /// <summary>
    /// Main entry point for the application
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize database on startup
            try
            {
                DatabaseManager.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database: {ex.Message}", 
                    "Database Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            // Start with login form
            Application.Run(new LoginForm());
        }
    }
}
