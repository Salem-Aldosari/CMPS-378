using System;

namespace BMICalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables to store user information
            string userName;
            int userAge;
            string userGender;
            int heightFeet;
            int heightInches;
            double userWeight;

            // Variables for calculations
            int totalInches;
            double bmiValue;
            string bmiCategory;

            // Ask user for their information
            Console.Write("Please enter your name: ");
            userName = Console.ReadLine();

            Console.Write("Please enter your age: ");
            userAge = int.Parse(Console.ReadLine());

            Console.Write("Please enter your Gender: ");
            userGender = Console.ReadLine();

            Console.Write("Please enter your height in feet: ");
            heightFeet = int.Parse(Console.ReadLine());

            Console.Write("Please enter your height in inches: ");
            heightInches = int.Parse(Console.ReadLine());

            Console.Write("Please enter your weight in pounds: ");
            userWeight = double.Parse(Console.ReadLine());

            // Convert height to total inches
            totalInches = (heightFeet * 12) + heightInches;

            // Calculate BMI using formula
            bmiValue = 703 * (userWeight / (totalInches * totalInches));

            // Determine BMI category using if-else
            if (bmiValue < 16)
            {
                bmiCategory = "Severe Thinness";
            }
            else if (bmiValue >= 16 && bmiValue < 17)
            {
                bmiCategory = "Moderate Thinness";
            }
            else if (bmiValue >= 17 && bmiValue < 18.5)
            {
                bmiCategory = "Mild Thinness";
            }
            else if (bmiValue >= 18.5 && bmiValue < 25)
            {
                bmiCategory = "Normal";
            }
            else if (bmiValue >= 25 && bmiValue < 30)
            {
                bmiCategory = "Overweight";
            }
            else if (bmiValue >= 30 && bmiValue < 35)
            {
                bmiCategory = "Obese Class I";
            }
            else if (bmiValue >= 35 && bmiValue < 40)
            {
                bmiCategory = "Obese Class II";
            }
            else
            {
                bmiCategory = "Obese Class III";
            }

            // Display output
            Console.WriteLine();
            Console.WriteLine("Hi " + userName + ",");
            Console.WriteLine();
            Console.WriteLine("You are a " + userGender + ". You are " + userAge + " years old. You are currently " + heightFeet + "'" + heightInches + "\" and you currently weight " + userWeight + " pounds. Your BMI is " + bmiValue.ToString("0.00") + ", which is " + bmiCategory + ".");
            Console.WriteLine();
            Console.WriteLine("Thank you for using the BMI Calculator!");
        }
    }
}
