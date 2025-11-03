using System;

namespace LADWPBillCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables for user input
            string customerName;
            double electricityUsage;
            double waterUsage;

            // Variables for rates and calculations
            double electricityRate;
            double waterRate;
            double electricityCost;
            double waterCost;
            double totalBill;

            // Display welcome message
            Console.WriteLine("****** Welcome to LADWP Utility Bill Calculator ******");
            Console.WriteLine();

            // Get user input
            Console.Write("Enter your name: ");
            customerName = Console.ReadLine();

            Console.Write("Enter electricity usage in kWh: ");
            electricityUsage = double.Parse(Console.ReadLine());

            Console.Write("Enter water usage in HCF: ");
            waterUsage = double.Parse(Console.ReadLine());

            // Determine electricity rate based on usage tiers
            if (electricityUsage >= 0 && electricityUsage <= 199)
            {
                electricityRate = 0.13;
            }
            else if (electricityUsage >= 200 && electricityUsage <= 499)
            {
                electricityRate = 0.17;
            }
            else if (electricityUsage >= 500 && electricityUsage <= 999)
            {
                electricityRate = 0.21;
            }
            else
            {
                electricityRate = 0.26;
            }

            // Determine water rate based on usage tiers
            if (waterUsage >= 0 && waterUsage <= 9)
            {
                waterRate = 2.30;
            }
            else if (waterUsage >= 10 && waterUsage <= 24)
            {
                waterRate = 3.10;
            }
            else if (waterUsage >= 25 && waterUsage <= 39)
            {
                waterRate = 4.20;
            }
            else
            {
                waterRate = 5.15;
            }

            // Calculate costs
            electricityCost = electricityUsage * electricityRate;
            waterCost = waterUsage * waterRate;
            totalBill = electricityCost + waterCost;

            // Display bill
            Console.WriteLine();
            Console.WriteLine("=========== LADWP MONTHLY BILL ===========");
            Console.WriteLine("Customer Name: " + customerName);
            Console.WriteLine();
            Console.WriteLine("Electricity Usage: " + electricityUsage + " kWh");
            Console.WriteLine("Rate Applied:      $" + electricityRate.ToString("0.00") + " per kWh");
            Console.WriteLine("Electricity Charge: $" + electricityCost.ToString("0.00"));
            Console.WriteLine();
            Console.WriteLine("Water Usage:   " + waterUsage + " HCF");
            Console.WriteLine("Rate Applied:  $" + waterRate.ToString("0.00") + " per HCF");
            Console.WriteLine("Water Charge:  $" + waterCost.ToString("0.00"));
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Total Amount Due: $" + totalBill.ToString("0.00"));
            Console.WriteLine("==========================================");
            Console.WriteLine("Thank you for using LADWP!");
        }
    }
}
