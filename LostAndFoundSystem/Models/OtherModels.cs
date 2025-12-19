using System;

namespace LostAndFoundSystem.Models
{
    /// <summary>
    /// Represents an item category
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents a campus location
    /// </summary>
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents a claim on an item
    /// </summary>
    public class Claim
    {
        public int ClaimID { get; set; }
        public int ItemID { get; set; }
        public int ClaimerUserID { get; set; }
        public string ClaimerName { get; set; }
        public DateTime ClaimDate { get; set; }
        public string VerificationStatus { get; set; } // Pending, Approved, Rejected
        public string VerifierNotes { get; set; }
        public string ContactInfo { get; set; }
        public string ItemDescription { get; set; }

        public Claim()
        {
            ClaimDate = DateTime.Now;
            VerificationStatus = "Pending";
        }
    }
}
