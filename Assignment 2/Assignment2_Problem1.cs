using System;

namespace FreelanceIncomeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables for user info
            string freelancerName;
            int numberOfProjects;

            // Variables for tracking
            double totalIncome = 0;
            double highestIncome = 0;
            string highestProject = "";

            // Display welcome message
            Console.WriteLine("****** Welcome to Freelance Project Income Tracker ******");
            Console.WriteLine();

            // Get freelancer name
            Console.Write("Enter your name: ");
            freelancerName = Console.ReadLine();

            // Get number of projects
            Console.Write("How many projects would you like to enter? ");
            numberOfProjects = int.Parse(Console.ReadLine());
            Console.WriteLine();

            // Loop through each project
            for (int i = 1; i <= numberOfProjects; i++)
            {
                // Variables for this project
                string projectName;
                double hoursWorked;
                double hourlyRate;
                double projectIncome;

                // Get project details
                Console.WriteLine("Project #" + i + ":");
                Console.Write("Enter project name: ");
                projectName = Console.ReadLine();

                Console.Write("Enter hours worked: ");
                hoursWorked = double.Parse(Console.ReadLine());

                Console.Write("Enter hourly rate: ");
                hourlyRate = double.Parse(Console.ReadLine());
                Console.WriteLine();

                // Calculate this project's income
                projectIncome = hoursWorked * hourlyRate;

                // Add to total income
                totalIncome = totalIncome + projectIncome;

                // Check if this is the highest earning project
                if (projectIncome > highestIncome)
                {
                    highestIncome = projectIncome;
                    highestProject = projectName;
                }
            }

            // Calculate average income per project
            double averageIncome = totalIncome / numberOfProjects;

            // Display summary
            Console.WriteLine("========== FREELANCE INCOME SUMMARY ==========");
            Console.WriteLine("Freelancer Name: " + freelancerName);
            Console.WriteLine("Projects Logged: " + numberOfProjects);
            Console.WriteLine();
            Console.WriteLine("Total Income: $" + totalIncome.ToString("0.00"));
            Console.WriteLine("Average Project Income: $" + averageIncome.ToString("0.00"));
            Console.WriteLine("Highest-Earning Project: " + highestProject + " ($" + highestIncome.ToString("0.00") + ")");
            Console.WriteLine("==============================================");
            Console.WriteLine("Thank you for using the Income Tracker!");
        }
    }
}
