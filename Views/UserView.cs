using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Controllers;
using Task_Console.Model;
using Task_Console.Utilities;

namespace Task_Console.Views
{
    public class UserView
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
        public static async Task<Tuple<string, string>> LoginUserView()
        {
            Console.WriteLine($"{lightBlue}Enter Your Login Details{resetColor}");
            Console.WriteLine($"{green}Enter your username:{resetColor}");
            string username =  AppView.GetUserInput();
            Console.WriteLine($"{green}Enter your password:{resetColor}");
            string password =  AppView.GetUserInput();
            if (Utility.ValidateStringInput(username, password))
            {
                return Tuple.Create(username, password);
            }
            else{
                AppView.ShowMessage($"{red}Invalid input. Please try again.{resetColor}");
                throw new ArgumentException($"{red}Invalid input.{resetColor}");
            }

        }

        public static async Task<Tuple<string, string>> RegisterUserView()
        {

            Console.WriteLine($"{orange}Enter Your Registration Details{resetColor}");
            Console.WriteLine($"{green}Enter your username:{resetColor}");
            string username =  Console.ReadLine();
            Console.WriteLine($"{green}Enter your password:{resetColor}");
            string password =  Console.ReadLine();
            if (Utility.ValidateStringInput(username, password))
            {
                return Tuple.Create(username, password);
            }
            else
            {
                AppView.ShowMessage($"{red}Invalid input. Please try again.{resetColor}");
                throw new ArgumentException("Invalid input.");
            }
        }

        public static async Task ShowUserTasksView(List<ProjectTask> tasks)
        {
            if (tasks.Count == 0)
            {
                AppView.ShowMessage($"{yellow}You don't have any tasks.{resetColor}");
                await UserController.ShowUserMenuGetOption();
            }
            else
            {
                Console.WriteLine($"{green}Here are your tasks:{resetColor}");
                foreach (ProjectTask task in tasks)
                {
                    task.printTask();
                }
            }
        }

        public static async Task ShowUserTaskView(ProjectTask task)
        {
            task.printTask();
        }

        public static async Task ShowUserProject(List<Project> projects)
        {
            if (projects.Count == 0)
            {
                AppView.ShowMessage($"{red}You don't have any projects.{resetColor}");
                UserController.ShowUserMenuGetOption();
            }
            else
            {
                Console.WriteLine($"{yellow}Here are your projects:{resetColor}");
                foreach (Project project in projects)
                {
                    project.PrintProject();
                }
            }
        }

        public static async Task ShowUserProjectView(List<Project> projects)
        {
            Console.WriteLine($"{lightBlue}Here are your projects:{resetColor}");
            foreach (Project project in projects)
            {
                project.PrintProject();
            }
        }

        public static async Task<string> ShowUserMenu()
        {
            Console.WriteLine("Welcome to the Task Manager");
            Console.WriteLine($"{blue}1{green} - Show Projects{resetColor}");
            Console.WriteLine($"{blue}2{green} - Show single Project{resetColor}");
            Console.WriteLine($"{blue}3{green} - Show Undone Tasks {resetColor}");
            Console.WriteLine($"{blue}4{green} - Show single Task {resetColor}");
            Console.WriteLine($"{blue}5{green} - Logout{resetColor}");
            string userInput = AppView.GetUserInput();
            if (int.TryParse(userInput, out int parsedInput) && Utility.ValidateRange(parsedInput, 1, 5))
            {
                return userInput;
            }
            else
            {
                AppView.ShowMessage($"{red}Invalid input. Please try again.{resetColor}");
                return await ShowUserMenu();
            }
        }

        public static async Task<string> CheckTaskDone(){
            Console.WriteLine($"{green}Mark Task as done{resetColor}");
            Console.WriteLine($"{yellow}Enter Task ID{resetColor}");
            string taskIdInput = AppView.GetUserInput();
            return taskIdInput;
        }
    }
}