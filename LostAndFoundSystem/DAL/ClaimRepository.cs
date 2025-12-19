using System;
using System.Collections.Generic;
using System.Data.SQLite;
using LostAndFoundSystem.Models;

namespace LostAndFoundSystem.DAL
{
    /// <summary>
    /// Repository for claim data access
    /// </summary>
    public class ClaimRepository
    {
        /// <summary>
        /// Add a new claim
        /// </summary>
        public int AddClaim(Claim claim)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = @"
                    INSERT INTO Claims (ItemID, ClaimerUserID, ClaimerName, ClaimDate, VerificationStatus, ContactInfo, ItemDescription)
                    VALUES (@ItemID, @ClaimerUserID, @ClaimerName, @ClaimDate, @VerificationStatus, @ContactInfo, @ItemDescription);
                    SELECT last_insert_rowid();";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemID", claim.ItemID);
                    command.Parameters.AddWithValue("@ClaimerUserID", claim.ClaimerUserID > 0 ? (object)claim.ClaimerUserID : DBNull.Value);
                    command.Parameters.AddWithValue("@ClaimerName", claim.ClaimerName);
                    command.Parameters.AddWithValue("@ClaimDate", claim.ClaimDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@VerificationStatus", claim.VerificationStatus);
                    command.Parameters.AddWithValue("@ContactInfo", claim.ContactInfo ?? "");
                    command.Parameters.AddWithValue("@ItemDescription", claim.ItemDescription ?? "");

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get all pending claims
        /// </summary>
        public List<Claim> GetPendingClaims()
        {
            return GetClaimsByStatus("Pending");
        }

        /// <summary>
        /// Get claims by status
        /// </summary>
        public List<Claim> GetClaimsByStatus(string status)
        {
            var claims = new List<Claim>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Claims WHERE VerificationStatus = @Status ORDER BY ClaimDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(new Claim
                            {
                                ClaimID = Convert.ToInt32(reader["ClaimID"]),
                                ItemID = Convert.ToInt32(reader["ItemID"]),
                                ClaimerUserID = reader["ClaimerUserID"] != DBNull.Value ? Convert.ToInt32(reader["ClaimerUserID"]) : 0,
                                ClaimerName = reader["ClaimerName"].ToString(),
                                ClaimDate = DateTime.Parse(reader["ClaimDate"].ToString()),
                                VerificationStatus = reader["VerificationStatus"].ToString(),
                                VerifierNotes = reader["VerifierNotes"]?.ToString() ?? "",
                                ContactInfo = reader["ContactInfo"]?.ToString() ?? "",
                                ItemDescription = reader["ItemDescription"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// Get claims for a specific item
        /// </summary>
        public List<Claim> GetClaimsByItemId(int itemId)
        {
            var claims = new List<Claim>();

            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Claims WHERE ItemID = @ItemID ORDER BY ClaimDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemID", itemId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(new Claim
                            {
                                ClaimID = Convert.ToInt32(reader["ClaimID"]),
                                ItemID = Convert.ToInt32(reader["ItemID"]),
                                ClaimerUserID = reader["ClaimerUserID"] != DBNull.Value ? Convert.ToInt32(reader["ClaimerUserID"]) : 0,
                                ClaimerName = reader["ClaimerName"].ToString(),
                                ClaimDate = DateTime.Parse(reader["ClaimDate"].ToString()),
                                VerificationStatus = reader["VerificationStatus"].ToString(),
                                VerifierNotes = reader["VerifierNotes"]?.ToString() ?? "",
                                ContactInfo = reader["ContactInfo"]?.ToString() ?? "",
                                ItemDescription = reader["ItemDescription"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// Update claim status
        /// </summary>
        public void UpdateClaimStatus(int claimId, string status, string verifierNotes)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Claims SET VerificationStatus = @Status, VerifierNotes = @VerifierNotes WHERE ClaimID = @ClaimID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@VerifierNotes", verifierNotes ?? "");
                    command.Parameters.AddWithValue("@ClaimID", claimId);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Check if item already has approved claim
        /// </summary>
        public bool HasApprovedClaim(int itemId)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Claims WHERE ItemID = @ItemID AND VerificationStatus = 'Approved'";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemID", itemId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
