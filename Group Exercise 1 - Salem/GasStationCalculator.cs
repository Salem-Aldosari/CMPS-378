using System;

namespace GasStationCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare variables to store user input
            string stationName;
            double pricePerGallon;
            double gallonsNeeded;
            double taxPercent;

            // Variables for calculations
            double beforeTax;
            double taxAmount;
            double finalTotal;

            // Get gas station name from user
            Console.Write("Enter the name of gas station: ");
            stationName = Console.ReadLine();

            // Get price per gallon from user
            Console.Write("Enter the price of gas: ");
            pricePerGallon = Convert.ToDouble(Console.ReadLine());

            // Get number of gallons from user
            Console.Write("How many gallons: ");
            gallonsNeeded = Convert.ToDouble(Console.ReadLine());

            // Get tax percentage from user
            Console.Write("What is the tax percentage for this transaction: ");
            taxPercent = Convert.ToDouble(Console.ReadLine());

            // Calculate the subtotal (price times gallons)
            beforeTax = pricePerGallon * gallonsNeeded;

            // Calculate the tax amount (subtotal times tax rate divided by 100)
            taxAmount = beforeTax * (taxPercent / 100);

            // Calculate the final total (subtotal plus tax)
            finalTotal = beforeTax + taxAmount;

            // Display the receipt
            Console.WriteLine();
            Console.WriteLine("******* " + stationName + " *******");
            Console.WriteLine("Gas price: $" + pricePerGallon.ToString("0.00"));
            Console.WriteLine("Number of gallons: " + gallonsNeeded);
            Console.WriteLine("Tax rate: " + taxPercent + "%");
            Console.WriteLine("Subtotal: $" + beforeTax.ToString("0.00"));
            Console.WriteLine("Tax: $" + taxAmount.ToString("0.00"));
            Console.WriteLine("Total: $" + finalTotal.ToString("0.00"));
        }
    }
}
