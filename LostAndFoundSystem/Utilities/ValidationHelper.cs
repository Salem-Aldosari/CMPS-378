using System;
using System.Windows.Forms;

namespace LostAndFoundSystem.Utilities
{
    /// <summary>
    /// Utility class for common validation methods
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Show error message box
        /// </summary>
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show success message box
        /// </summary>
        public static void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show warning message box
        /// </summary>
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show confirmation dialog
        /// </summary>
        public static bool ShowConfirmation(string message)
        {
            var result = MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Validate required field
        /// </summary>
        public static bool IsRequired(TextBox textBox, string fieldName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorMessage = $"{fieldName} is required.";
                textBox.Focus();
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validate ComboBox selection
        /// </summary>
        public static bool IsSelected(ComboBox comboBox, string fieldName, out string errorMessage)
        {
            if (comboBox.SelectedIndex < 0 || comboBox.SelectedValue == null)
            {
                errorMessage = $"Please select a {fieldName}.";
                comboBox.Focus();
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Format date for display
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mm tt");
        }

        /// <summary>
        /// Get days ago text
        /// </summary>
        public static string GetDaysAgo(DateTime date)
        {
            int days = (DateTime.Now - date).Days;
            
            if (days == 0)
                return "Today";
            else if (days == 1)
                return "Yesterday";
            else
                return $"{days} days ago";
        }
    }
}
