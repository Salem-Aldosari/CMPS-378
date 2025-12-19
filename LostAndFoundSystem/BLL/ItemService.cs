using System;
using System.Collections.Generic;
using System.Linq;
using LostAndFoundSystem.Models;
using LostAndFoundSystem.DAL;

namespace LostAndFoundSystem.BLL
{
    /// <summary>
    /// Service for item management business logic
    /// </summary>
    public class ItemService
    {
        private readonly ItemRepository _itemRepository;
        private readonly DuplicateDetectionService _duplicateDetectionService;

        public ItemService()
        {
            _itemRepository = new ItemRepository();
            _duplicateDetectionService = new DuplicateDetectionService();
        }

        /// <summary>
        /// Add a new found item
        /// </summary>
        public bool AddItem(Item item, out string errorMessage, out List<Item> duplicates)
        {
            duplicates = new List<Item>();
            errorMessage = string.Empty;

            // Validate item
            if (!item.IsValid(out errorMessage))
            {
                return false;
            }

            // Check for duplicates
            duplicates = _duplicateDetectionService.FindPotentialDuplicates(item);

            // Add item to database
            try
            {
                item.ItemID = _itemRepository.AddItem(item);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error adding item: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Search items with filters
        /// </summary>
        public List<Item> SearchItems(string keyword, int? categoryId, int? locationId, string status)
        {
            return _itemRepository.SearchItems(keyword, categoryId, locationId, status);
        }

        /// <summary>
        /// Get all items
        /// </summary>
        public List<Item> GetAllItems()
        {
            return _itemRepository.GetAllItems();
        }

        /// <summary>
        /// Get item by ID
        /// </summary>
        public Item GetItemById(int itemId)
        {
            return _itemRepository.GetItemById(itemId);
        }

        /// <summary>
        /// Update item status
        /// </summary>
        public bool UpdateItemStatus(int itemId, string status, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                _itemRepository.UpdateItemStatus(itemId, status);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error updating item status: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Get items older than specified days
        /// </summary>
        public List<Item> GetOldItems(int days)
        {
            return _itemRepository.GetItemsByAgeThreshold(days);
        }

        /// <summary>
        /// Get item count statistics
        /// </summary>
        public Dictionary<string, int> GetItemStatistics()
        {
            return _itemRepository.GetItemCountByStatus();
        }

        /// <summary>
        /// Get items by category distribution
        /// </summary>
        public Dictionary<string, int> GetCategoryDistribution()
        {
            var items = _itemRepository.GetAllItems();
            return items.GroupBy(i => i.CategoryName)
                       .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Mark item as claimed
        /// </summary>
        public bool ClaimItem(int itemId, out string errorMessage)
        {
            return UpdateItemStatus(itemId, "Claimed", out errorMessage);
        }

        /// <summary>
        /// Archive old items
        /// </summary>
        public int ArchiveOldItems(int daysThreshold)
        {
            var oldItems = _itemRepository.GetItemsByAgeThreshold(daysThreshold);
            int archivedCount = 0;

            foreach (var item in oldItems)
            {
                if (UpdateItemStatus(item.ItemID, "Archived", out _))
                {
                    archivedCount++;
                }
            }

            return archivedCount;
        }
    }
}
