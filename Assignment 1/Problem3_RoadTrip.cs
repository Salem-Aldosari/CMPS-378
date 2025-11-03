using System;

namespace RoadTripBudget
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables for user input
            string travelerName;
            int tripDays;
            double totalMiles;
            double carMPG;
            double gasPricePerGallon;
            double hotelPerNight;
            double foodPerDay;

            // Variables for calculations
            double gallonsNeeded;
            double gasCost;
            double hotelTotal;
            double foodTotal;
            double tripTotal;

            // Display welcome message
            Console.WriteLine("****** Welcome to the Road Trip Budget Estimator ******");
            Console.WriteLine();

            // Get user input
            Console.Write("Enter your name: ");
            travelerName = Console.ReadLine();

            Console.Write("How many days will your trip be? ");
            tripDays = int.Parse(Console.ReadLine());

            Console.Write("How many miles will you drive in total? ");
            totalMiles = double.Parse(Console.ReadLine());

            Console.Write("What is your car's MPG? ");
            carMPG = double.Parse(Console.ReadLine());

            Console.Write("What is the average gas price per gallon? ");
            gasPricePerGallon = double.Parse(Console.ReadLine());

            Console.Write("What is your nightly hotel cost? ");
            hotelPerNight = double.Parse(Console.ReadLine());

            Console.Write("What is your daily food budget? ");
            foodPerDay = double.Parse(Console.ReadLine());

            // Perform calculations
            gallonsNeeded = totalMiles / carMPG;
            gasCost = gallonsNeeded * gasPricePerGallon;
            hotelTotal = hotelPerNight * (tripDays - 1);
            foodTotal = foodPerDay * tripDays;
            tripTotal = gasCost + hotelTotal + foodTotal;

            // Display results
            Console.WriteLine();
            Console.WriteLine("=========== ROAD TRIP BUDGET SUMMARY ===========");
            Console.WriteLine("Name:           " + travelerName);
            Console.WriteLine("Trip Duration:  " + tripDays + " days");
            Console.WriteLine("Total Miles:    " + totalMiles + " miles");
            Console.WriteLine("Fuel Efficiency: " + carMPG + " MPG");
            Console.WriteLine("Gas Price:      $" + gasPricePerGallon.ToString("0.00"));
            Console.WriteLine("Hotel Cost/Night: $" + hotelPerNight.ToString("0.00"));
            Console.WriteLine("Daily Food Budget: $" + foodPerDay.ToString("0.00"));
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Gas Needed:     " + gallonsNeeded.ToString("0.00") + " gallons");
            Console.WriteLine("Estimated Gas Cost: $" + gasCost.ToString("0.00"));
            Console.WriteLine("Estimated Hotel Cost: $" + hotelTotal.ToString("0.00"));
            Console.WriteLine("Estimated Food Cost: $" + foodTotal.ToString("0.00"));
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Estimated Trip Total: $" + tripTotal.ToString("0.00"));
            Console.WriteLine("===============================================");
            Console.WriteLine("Thanks for using the Road Trip Budget Estimator!");
        }
    }
}
