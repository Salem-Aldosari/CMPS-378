using System;

namespace MonthlyExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables for user info
            string userName;

            // Variables for tracking expenses
            double totalAmount = 0;
            int expenseCount = 0;
            double currentAmount;

            // Variables for tracking categories
            string highestCategory = "";
            double highestCategoryAmount = 0;

            // Variables to track each category total
            double rentTotal = 0;
            double groceriesTotal = 0;
            double utilitiesTotal = 0;
            double entertainmentTotal = 0;
            double otherTotal = 0;

            // Display welcome message
            Console.WriteLine("****** Welcome to Monthly Expense Tracker ******");
            Console.WriteLine();

            // Get user name
            Console.Write("Enter your name: ");
            userName = Console.ReadLine();
            Console.WriteLine();

            // Loop to get expenses until -1 is entered
            while (true)
            {
                string category;

                // Get expense category
                Console.Write("Enter expense category (or type 'done' to stop): ");
                category = Console.ReadLine();

                // Check if user wants to stop
                if (category == "done" || category == "Done")
                {
                    break;
                }

                // Get expense amount
                Console.Write("Enter amount: ");
                currentAmount = double.Parse(Console.ReadLine());

                // Check for sentinel value
                if (currentAmount == -1)
                {
                    break;
                }

                Console.WriteLine();

                // Add to total
                totalAmount = totalAmount + currentAmount;
                expenseCount = expenseCount + 1;

                // Add to category totals
                if (category == "Rent" || category == "rent")
                {
                    rentTotal = rentTotal + currentAmount;
                }
                else if (category == "Groceries" || category == "groceries")
                {
                    groceriesTotal = groceriesTotal + currentAmount;
                }
                else if (category == "Utilities" || category == "utilities")
                {
                    utilitiesTotal = utilitiesTotal + currentAmount;
                }
                else if (category == "Entertainment" || category == "entertainment")
                {
                    entertainmentTotal = entertainmentTotal + currentAmount;
                }
                else
                {
                    otherTotal = otherTotal + currentAmount;
                }
            }

            // Find highest spending category
            highestCategory = "Rent";
            highestCategoryAmount = rentTotal;

            if (groceriesTotal > highestCategoryAmount)
            {
                highestCategory = "Groceries";
                highestCategoryAmount = groceriesTotal;
            }

            if (utilitiesTotal > highestCategoryAmount)
            {
                highestCategory = "Utilities";
                highestCategoryAmount = utilitiesTotal;
            }

            if (entertainmentTotal > highestCategoryAmount)
            {
                highestCategory = "Entertainment";
                highestCategoryAmount = entertainmentTotal;
            }

            if (otherTotal > highestCategoryAmount)
            {
                highestCategory = "Other";
                highestCategoryAmount = otherTotal;
            }

            // Calculate average expense
            double averageExpense = 0;
            if (expenseCount > 0)
            {
                averageExpense = totalAmount / expenseCount;
            }

            // Display summary
            Console.WriteLine();
            Console.WriteLine("========== MONTHLY EXPENSE SUMMARY ==========");
            Console.WriteLine("Name:           " + userName);
            Console.WriteLine("Total Entries:  " + expenseCount);
            Console.WriteLine();
            Console.WriteLine("Total Amount Spent: $" + totalAmount.ToString("0.00"));
            Console.WriteLine("Average Expense:    $" + averageExpense.ToString("0.00"));
            Console.WriteLine("Highest Spending Category: " + highestCategory);
            Console.WriteLine("============================================");
            Console.WriteLine("Thank you for using the Monthly Expense Tracker!");
        }
    }
}
