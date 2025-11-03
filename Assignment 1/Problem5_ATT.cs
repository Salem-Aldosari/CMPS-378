using System;

namespace ATTFiberBilling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables for user input
            string customerName;
            int planSpeed;
            string addStaticIP;

            // Variables for pricing and calculations
            double basePrice;
            double staticIPCost = 0;
            double subtotalAmount;
            double taxAmount;
            double totalAmount;

            // Display welcome message
            Console.WriteLine("****** Welcome to AT&T Fiber Internet Billing System ******");
            Console.WriteLine();

            // Get user input
            Console.Write("Enter your name: ");
            customerName = Console.ReadLine();

            Console.Write("Select internet plan speed (300 / 500 / 1000 / 2000 / 5000 Mbps): ");
            planSpeed = int.Parse(Console.ReadLine());

            Console.Write("Would you like to add a static IP? (Y/N): ");
            addStaticIP = Console.ReadLine();

            // Determine base price based on plan speed
            if (planSpeed == 300)
            {
                basePrice = 55.00;
            }
            else if (planSpeed == 500)
            {
                basePrice = 65.00;
            }
            else if (planSpeed == 1000)
            {
                basePrice = 80.00;
            }
            else if (planSpeed == 2000)
            {
                basePrice = 110.00;
            }
            else if (planSpeed == 5000)
            {
                basePrice = 180.00;
            }
            else
            {
                basePrice = 0;
            }

            // Add static IP cost if selected
            if (addStaticIP == "Y" || addStaticIP == "y")
            {
                staticIPCost = 15.00;
            }

            // Calculate totals
            subtotalAmount = basePrice + staticIPCost;
            taxAmount = subtotalAmount * 0.095;
            totalAmount = subtotalAmount + taxAmount;

            // Display bill
            Console.WriteLine();
            Console.WriteLine("=========== AT&T FIBER BILLING SUMMARY ===========");
            Console.WriteLine("Customer Name:  " + customerName);
            Console.WriteLine();
            Console.WriteLine("Plan Speed:     " + planSpeed + " Mbps");
            Console.WriteLine("Base Price:     $" + basePrice.ToString("0.00"));
            Console.WriteLine("Static IP:      $" + staticIPCost.ToString("0.00"));
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Subtotal:       $" + subtotalAmount.ToString("0.00"));
            Console.WriteLine("Tax (9.5%):     $" + taxAmount.ToString("0.00"));
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Total Due:      $" + totalAmount.ToString("0.00"));
            Console.WriteLine("==================================================");
            Console.WriteLine("Thank you for choosing AT&T Fiber!");
        }
    }
}
