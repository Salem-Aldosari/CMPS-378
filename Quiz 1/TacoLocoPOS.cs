using System;

namespace TacoLocoPOS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print welcome message
            Console.WriteLine("****** Welcome to Taco Loco POS System ******");
            Console.WriteLine("Menu:");
            Console.WriteLine("A - 3 Tacos Combo - $7.00 / $9.00 with drink");
            Console.WriteLine("B - Burrito Meal - $8.50 / $10.50 with drink");
            Console.WriteLine("C - Quesadilla Special - $6.00 / $8.00 with drink");
            Console.WriteLine("D - Loaded Nachos - $5.75 / $7.75 with drink");
            Console.WriteLine();

            // Ask for customer name
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            // Ask for package code
            Console.Write("Enter your package code (A/B/C/D): ");
            string code = Console.ReadLine();
            code = code.ToUpper(); // Make it uppercase

            // Ask if they want a drink
            Console.Write("Would you like to add a drink? (Y/N): ");
            string drinkAnswer = Console.ReadLine();
            drinkAnswer = drinkAnswer.ToUpper(); // Make it uppercase

            // Ask how many they want
            Console.Write("How many combos would you like? ");
            string quantityInput = Console.ReadLine();
            int howMany = int.Parse(quantityInput);

            // Variables to store price and meal name
            double itemCost = 0;
            string mealName = "";

            // Figure out which meal they ordered and the price
            if (code == "A")
            {
                mealName = "3 Tacos Combo";
                if (drinkAnswer == "Y")
                {
                    itemCost = 9.00;
                }
                else
                {
                    itemCost = 7.00;
                }
            }
            else if (code == "B")
            {
                mealName = "Burrito Meal";
                if (drinkAnswer == "Y")
                {
                    itemCost = 10.50;
                }
                else
                {
                    itemCost = 8.50;
                }
            }
            else if (code == "C")
            {
                mealName = "Quesadilla Special";
                if (drinkAnswer == "Y")
                {
                    itemCost = 8.00;
                }
                else
                {
                    itemCost = 6.00;
                }
            }
            else if (code == "D")
            {
                mealName = "Loaded Nachos";
                if (drinkAnswer == "Y")
                {
                    itemCost = 7.75;
                }
                else
                {
                    itemCost = 5.75;
                }
            }

            // Calculate the money amounts
            double beforeTax = itemCost * howMany;
            double taxAmount = beforeTax * 0.095;
            double finalTotal = beforeTax + taxAmount;

            // Check if they get free dessert
            string drinkText = "";
            if (drinkAnswer == "Y")
            {
                drinkText = "Yes";
            }
            else
            {
                drinkText = "No";
            }

            // Print the receipt
            Console.WriteLine();
            Console.WriteLine("=========== TACO LOCO RECEIPT ===========");
            Console.WriteLine("Customer Name: " + name);
            Console.WriteLine("Combo Ordered: " + mealName);
            Console.WriteLine("Drink Included: " + drinkText);
            Console.WriteLine("Quantity: " + howMany);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Price Per Item: $" + itemCost);
            Console.WriteLine("Subtotal: $" + beforeTax);
            Console.WriteLine("Tax (9.5%): $" + taxAmount);
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Total Cost: $" + finalTotal);

            // Check if they earned a free dessert
            if (beforeTax >= 20.00)
            {
                Console.WriteLine("Congratulations! You get a free dessert!");
            }

            Console.WriteLine("=========================================");
            Console.WriteLine("Thank you for supporting Taco Loco!");
        }
    }
}
