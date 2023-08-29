using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Utilities;
using Task_Console.Controllers;

namespace Task_Console.Views
{
    public class AppView
    {
        private static string white = "\u001b[37m";
        private static string blue = "\u001b[34m";
        private static string green = "\u001b[32m";
        private static string yellow = "\u001b[33m";
        private static string magenta = "\u001b[35m";
        private static string red = "\u001b[31m";
        private static string resetColor = "\u001b[0m";
        private static string lightBlue = "\u001b[36m";
        private static string orange = "\u001b[38;5;208m";
        public static async Task InitApp()
        {
            Console.Clear();
            Console.WriteLine($"{blue}1.{yellow} Register user{resetColor}");
            Console.WriteLine($"{blue}2.{yellow} Login user{resetColor}");
            Console.WriteLine($"{blue}3.{yellow} Admin login{resetColor}");
            Console.WriteLine($"{blue}4.{yellow} Register admin{resetColor}"); // Add admin registration option
            Console.WriteLine($"{blue}5.{yellow} Exit{resetColor}");
            Console.WriteLine($"{green}Enter your choice{resetColor}");
            string option = GetUserInput();

            if (int.TryParse(option, out int choice))
            {
                if (Utility.ValidateRange(choice, 1, 5))
                {
                    if (choice == 3)
                    {
                        await AdminController.AdminBaseRouter(choice);
                    }
                    else if (choice == 4) // Handle admin registration option
                    {
                        await AdminController.AdminBaseRouter(choice);
                    }
                    else
                    {
                        await UserController.UserBaseRouter(option);
                    }
                }
                else
                {
                    AppView.ShowMessage($"{red}Invalid choice. Please enter a number between 1 and 5.{resetColor}");
                    await InitApp();
                }
            }
            else
            {
                AppView.ShowMessage($"{red}Invalid input. Please enter a valid number.{red}");
                await InitApp();
            }
        }

        
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        //  get user input
        public static string GetUserInput()
        {
            return  Console.ReadLine();
        }
    }
}