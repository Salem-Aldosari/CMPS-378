using System;

namespace LostAndFoundSystem.Models
{
    /// <summary>
    /// Represents a found item in the system
    /// </summary>
    public class Item
    {
        public int ItemID { get; set; }
        public string Title { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime FoundDate { get; set; }
        public int FoundLocationID { get; set; }
        public string LocationName { get; set; }
        public string Status { get; set; } // Active, Claimed, Archived
        public string FinderNotes { get; set; }
        public int LoggedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public Item()
        {
            CreatedDate = DateTime.Now;
            Status = "Active";
        }

        /// <summary>
        /// Validates that all required fields are filled
        /// </summary>
        public bool IsValid(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                errorMessage = "Title is required.";
                return false;
            }

            if (CategoryID <= 0)
            {
                errorMessage = "Please select a category.";
                return false;
            }

            if (FoundLocationID <= 0)
            {
                errorMessage = "Please select a location.";
                return false;
            }

            if (FoundDate > DateTime.Now)
            {
                errorMessage = "Found date cannot be in the future.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
