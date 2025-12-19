using System;
using System.Collections.Generic;
using LostAndFoundSystem.Models;
using LostAndFoundSystem.DAL;

namespace LostAndFoundSystem.BLL
{
    /// <summary>
    /// Service for claim management business logic
    /// </summary>
    public class ClaimService
    {
        private readonly ClaimRepository _claimRepository;
        private readonly ItemRepository _itemRepository;

        public ClaimService()
        {
            _claimRepository = new ClaimRepository();
            _itemRepository = new ItemRepository();
        }

        /// <summary>
        /// Submit a new claim
        /// </summary>
        public bool SubmitClaim(Claim claim, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validate claim
            if (string.IsNullOrWhiteSpace(claim.ClaimerName))
            {
                errorMessage = "Claimer name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(claim.ContactInfo))
            {
                errorMessage = "Contact information is required.";
                return false;
            }

            // Check if item already has an approved claim
            if (_claimRepository.HasApprovedClaim(claim.ItemID))
            {
                errorMessage = "This item has already been claimed.";
                return false;
            }

            try
            {
                claim.ClaimID = _claimRepository.AddClaim(claim);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error submitting claim: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Get all pending claims
        /// </summary>
        public List<Claim> GetPendingClaims()
        {
            return _claimRepository.GetPendingClaims();
        }

        /// <summary>
        /// Get claims for a specific item
        /// </summary>
        public List<Claim> GetClaimsByItemId(int itemId)
        {
            return _claimRepository.GetClaimsByItemId(itemId);
        }

        /// <summary>
        /// Approve a claim
        /// </summary>
        public bool ApproveClaim(int claimId, string verifierNotes, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                _claimRepository.UpdateClaimStatus(claimId, "Approved", verifierNotes);
                
                // Update item status to Claimed
                var claims = _claimRepository.GetClaimsByStatus("Approved");
                var claim = claims.Find(c => c.ClaimID == claimId);
                if (claim != null)
                {
                    _itemRepository.UpdateItemStatus(claim.ItemID, "Claimed");
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error approving claim: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Reject a claim
        /// </summary>
        public bool RejectClaim(int claimId, string verifierNotes, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                _claimRepository.UpdateClaimStatus(claimId, "Rejected", verifierNotes);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error rejecting claim: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Check if an item can be claimed
        /// </summary>
        public bool CanClaimItem(int itemId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var item = _itemRepository.GetItemById(itemId);
            if (item == null)
            {
                errorMessage = "Item not found.";
                return false;
            }

            if (item.Status == "Claimed")
            {
                errorMessage = "This item has already been claimed.";
                return false;
            }

            if (item.Status == "Archived")
            {
                errorMessage = "This item has been archived and can no longer be claimed.";
                return false;
            }

            if (_claimRepository.HasApprovedClaim(itemId))
            {
                errorMessage = "This item has an approved claim.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get claim statistics
        /// </summary>
        public Dictionary<string, int> GetClaimStatistics()
        {
            var stats = new Dictionary<string, int>
            {
                { "Pending", _claimRepository.GetClaimsByStatus("Pending").Count },
                { "Approved", _claimRepository.GetClaimsByStatus("Approved").Count },
                { "Rejected", _claimRepository.GetClaimsByStatus("Rejected").Count }
            };

            return stats;
        }
    }
}
