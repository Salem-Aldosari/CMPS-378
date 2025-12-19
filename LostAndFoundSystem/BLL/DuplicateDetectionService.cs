using System;
using System.Collections.Generic;
using System.Linq;
using LostAndFoundSystem.Models;
using LostAndFoundSystem.DAL;

namespace LostAndFoundSystem.BLL
{
    /// <summary>
    /// Service for detecting potential duplicate items
    /// </summary>
    public class DuplicateDetectionService
    {
        private readonly ItemRepository _itemRepository;
        private const int DaysToCheck = 7; // Check for duplicates in last 7 days
        private const double SimilarityThreshold = 0.70; // 70% similarity score

        public DuplicateDetectionService()
        {
            _itemRepository = new ItemRepository();
        }

        /// <summary>
        /// Find potential duplicate items
        /// </summary>
        public List<Item> FindPotentialDuplicates(Item newItem)
        {
            // Get recent items in the same category
            var recentItems = _itemRepository.GetRecentItemsByCategory(newItem.CategoryID, DaysToCheck);
            
            if (recentItems == null || !recentItems.Any())
            {
                return new List<Item>();
            }

            var potentialDuplicates = new List<Item>();

            foreach (var item in recentItems)
            {
                double similarityScore = CalculateSimilarityScore(newItem, item);

                if (similarityScore >= SimilarityThreshold)
                {
                    potentialDuplicates.Add(item);
                }
            }

            return potentialDuplicates;
        }

        /// <summary>
        /// Calculate similarity score between two items
        /// </summary>
        private double CalculateSimilarityScore(Item item1, Item item2)
        {
            double keywordScore = CalculateKeywordSimilarity(item1, item2) * 0.4;
            double locationScore = (item1.FoundLocationID == item2.FoundLocationID) ? 0.3 : 0.0;
            double descriptionScore = CalculateDescriptionSimilarity(item1, item2) * 0.3;

            return keywordScore + locationScore + descriptionScore;
        }

        /// <summary>
        /// Calculate keyword similarity in titles
        /// </summary>
        private double CalculateKeywordSimilarity(Item item1, Item item2)
        {
            var title1Words = GetWords(item1.Title);
            var title2Words = GetWords(item2.Title);

            if (!title1Words.Any() || !title2Words.Any())
            {
                return 0.0;
            }

            int matchingWords = title1Words.Intersect(title2Words, StringComparer.OrdinalIgnoreCase).Count();
            int totalWords = Math.Max(title1Words.Count, title2Words.Count);

            return (double)matchingWords / totalWords;
        }

        /// <summary>
        /// Calculate description similarity
        /// </summary>
        private double CalculateDescriptionSimilarity(Item item1, Item item2)
        {
            if (string.IsNullOrWhiteSpace(item1.Description) || string.IsNullOrWhiteSpace(item2.Description))
            {
                return 0.0;
            }

            var desc1Words = GetWords(item1.Description);
            var desc2Words = GetWords(item2.Description);

            if (!desc1Words.Any() || !desc2Words.Any())
            {
                return 0.0;
            }

            int matchingWords = desc1Words.Intersect(desc2Words, StringComparer.OrdinalIgnoreCase).Count();
            int totalWords = Math.Max(desc1Words.Count, desc2Words.Count);

            return (double)matchingWords / totalWords;
        }

        /// <summary>
        /// Extract words from text
        /// </summary>
        private List<string> GetWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string>();
            }

            // Remove special characters and split into words
            var words = text.ToLower()
                           .Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '-', '(', ')' }, 
                                  StringSplitOptions.RemoveEmptyEntries)
                           .Where(w => w.Length > 2) // Ignore very short words
                           .ToList();

            return words;
        }

        /// <summary>
        /// Check if an item is likely a duplicate (high similarity)
        /// </summary>
        public bool IsLikelyDuplicate(Item newItem)
        {
            var duplicates = FindPotentialDuplicates(newItem);
            return duplicates.Any();
        }
    }
}
