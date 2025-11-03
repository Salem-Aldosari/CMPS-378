using System;

namespace BirthdayMeaning
{
    class Program
    {
        static void Main(string[] args)
        {
            string keepGoing = "Y";

            // Welcome message
            Console.WriteLine("Welcome to Birthday Date Meaning Generator!");
            Console.WriteLine();

            // While loop to allow multiple tries
            while (keepGoing == "Y" || keepGoing == "y")
            {
                // Variables for birthday
                int birthMonth;
                int birthDay;
                int birthYear;

                // Variables for meanings
                string monthMeaning = "";
                string dayMeaning = "";
                string yearMeaning = "";

                // Get birthday information
                Console.Write("Please enter the month of your birthday: ");
                birthMonth = int.Parse(Console.ReadLine());

                Console.Write("Please enter the day of your birthday: ");
                birthDay = int.Parse(Console.ReadLine());

                Console.Write("Please enter the year of your birthday: ");
                birthYear = int.Parse(Console.ReadLine());

                // Determine month meaning using switch
                switch (birthMonth)
                {
                    case 1:
                        monthMeaning = "January means Janus";
                        break;
                    case 2:
                        monthMeaning = "February means Purification";
                        break;
                    case 3:
                        monthMeaning = "March means Mars";
                        break;
                    case 4:
                        monthMeaning = "April means Opening";
                        break;
                    case 5:
                        monthMeaning = "May means Maia";
                        break;
                    case 6:
                        monthMeaning = "June means Juno";
                        break;
                    case 7:
                        monthMeaning = "July means Julius Caesar";
                        break;
                    case 8:
                        monthMeaning = "August means Augustus";
                        break;
                    case 9:
                        monthMeaning = "September means Seventh Month";
                        break;
                    case 10:
                        monthMeaning = "October means Eighth Month";
                        break;
                    case 11:
                        monthMeaning = "November means Ninth Month";
                        break;
                    case 12:
                        monthMeaning = "December means Tenth Month";
                        break;
                }

                // Determine day meaning using switch
                switch (birthDay)
                {
                    case 1:
                        dayMeaning = "Self-Starter";
                        break;
                    case 2:
                        dayMeaning = "Peacemaker";
                        break;
                    case 3:
                        dayMeaning = "Creative";
                        break;
                    case 4:
                        dayMeaning = "Practical";
                        break;
                    case 5:
                        dayMeaning = "Adventurous";
                        break;
                    case 6:
                        dayMeaning = "Nurturing";
                        break;
                    case 7:
                        dayMeaning = "Analytical";
                        break;
                    case 8:
                        dayMeaning = "Ambitious";
                        break;
                    case 9:
                        dayMeaning = "Humanitarian";
                        break;
                    case 10:
                        dayMeaning = "Independent";
                        break;
                    case 11:
                        dayMeaning = "Intuitive";
                        break;
                    case 12:
                        dayMeaning = "Expressive";
                        break;
                    case 13:
                        dayMeaning = "Hardworking";
                        break;
                    case 14:
                        dayMeaning = "Freedom-loving";
                        break;
                    case 15:
                        dayMeaning = "Responsible";
                        break;
                    case 16:
                        dayMeaning = "Perfectionist";
                        break;
                    case 17:
                        dayMeaning = "Visionary";
                        break;
                    case 18:
                        dayMeaning = "Influential";
                        break;
                    case 19:
                        dayMeaning = "Dynamic";
                        break;
                    case 20:
                        dayMeaning = "Sensitive";
                        break;
                    case 21:
                        dayMeaning = "Social";
                        break;
                    case 22:
                        dayMeaning = "Master Builder";
                        break;
                    case 23:
                        dayMeaning = "Versatile";
                        break;
                    case 24:
                        dayMeaning = "Family-oriented";
                        break;
                    case 25:
                        dayMeaning = "Thoughtful";
                        break;
                    case 26:
                        dayMeaning = "Business-minded";
                        break;
                    case 27:
                        dayMeaning = "Compassionate";
                        break;
                    case 28:
                        dayMeaning = "Leader";
                        break;
                    case 29:
                        dayMeaning = "Spiritual";
                        break;
                    case 30:
                        dayMeaning = "Communicator";
                        break;
                    case 31:
                        dayMeaning = "Organized";
                        break;
                }

                // Determine year meaning using switch
                switch (birthYear)
                {
                    case 2000:
                    case 2001:
                    case 2002:
                    case 2003:
                    case 2004:
                    case 2005:
                    case 2006:
                    case 2007:
                    case 2008:
                    case 2009:
                    case 2010:
                        yearMeaning = "that you are millennial";
                        break;
                    case 2011:
                    case 2012:
                    case 2013:
                    case 2014:
                    case 2015:
                    case 2016:
                    case 2017:
                    case 2018:
                    case 2019:
                    case 2020:
                    case 2021:
                    case 2022:
                    case 2023:
                        yearMeaning = "that you are Gen Z";
                        break;
                }

                // Display results
                Console.WriteLine();
                Console.WriteLine("The month of " + GetMonthName(birthMonth) + " means " + monthMeaning);
                Console.WriteLine("The " + birthDay + GetDaySuffix(birthDay) + " of " + GetMonthName(birthMonth) + " means " + dayMeaning);
                Console.WriteLine("The year of " + birthYear + " means " + yearMeaning);
                Console.WriteLine();

                // Ask if user wants to try again
                Console.Write("Would you like to try another one? ");
                keepGoing = Console.ReadLine();
                Console.WriteLine();
            }

            Console.WriteLine("Thanks for playing!");
        }

        // Helper function to get month name
        static string GetMonthName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1: monthName = "January"; break;
                case 2: monthName = "February"; break;
                case 3: monthName = "March"; break;
                case 4: monthName = "April"; break;
                case 5: monthName = "May"; break;
                case 6: monthName = "June"; break;
                case 7: monthName = "July"; break;
                case 8: monthName = "August"; break;
                case 9: monthName = "September"; break;
                case 10: monthName = "October"; break;
                case 11: monthName = "November"; break;
                case 12: monthName = "December"; break;
            }
            return monthName;
        }

        // Helper function to get day suffix (st, nd, rd, th)
        static string GetDaySuffix(int day)
        {
            if (day == 1 || day == 21 || day == 31)
                return "st";
            else if (day == 2 || day == 22)
                return "nd";
            else if (day == 3 || day == 23)
                return "rd";
            else
                return "th";
        }
    }
}
