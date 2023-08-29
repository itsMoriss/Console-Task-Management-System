using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Controllers;
using Task_Console.Model;
using Task_Console.Utilities;

namespace Task_Console.Views
{
    public class AdminView
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

        private static bool isInitialAdminCreated = false;
        private static string initialAdminUsername;
        private static string initialAdminPassword;
        private static Admin loginAdmin;

        public static async Task<Tuple<string, string>> RegisterAdminView()
        {
            Console.WriteLine("Enter Admin Registration Details");
            Console.WriteLine($"{green}Enter admin username:{resetColor}");
            string username = Console.ReadLine();
            Console.WriteLine($"{green}Enter admin password:{resetColor}");
            string password = Console.ReadLine();

            if (Utility.ValidateStringInput(username, password))
            {
                return Tuple.Create(username, password);
            }
            else
            {
                AppView.ShowMessage("Invalid input. Please try again.");
                throw new ArgumentException("Invalid input.");
            }
        }

        public static async Task<Tuple<string, string>> AdminLoginView()
        {
            Console.WriteLine($"{orange}Enter Admin Login Details{resetColor}");
            Console.WriteLine($"{green}Enter admin username:{resetColor}");
            string username = Console.ReadLine();
            Console.WriteLine($"{green}Enter admin password:{resetColor}");
            string password = Console.ReadLine();

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


        // public static async Task<Admin> CreateInitialAdminView()
        // {
        // // Console.WriteLine("Calling CreateInitialAdminView");

        //     if (isInitialAdminCreated)
        //     {
        //         // Console.WriteLine("Initial Admin Account Setup - Step 1");
        //         Console.WriteLine("Initial admin account has already been set up.");
        //     }
        //     else
        //     {
        //         Console.WriteLine("Initial Admin Account Setup");
        //         Console.WriteLine("Enter admin username:");
        //         initialAdminUsername = GetUserInput();
        //         Console.WriteLine("Enter admin password:");
        //         initialAdminPassword = GetUserInput();
        //         isInitialAdminCreated = true;
        //         loginAdmin = new Admin { Username = initialAdminUsername, Password = initialAdminPassword };
        //     }

        //     Console.WriteLine("Enter admin username:");
        //     string username = GetUserInput();
        //     Console.WriteLine("Enter admin password:");
        //     string password = GetUserInput();

        //     if (username == initialAdminUsername && password == initialAdminPassword)
        //     {
        //         Console.WriteLine($"Username: {username}");
        //         Console.WriteLine($"Password: {password}");
        //         Console.WriteLine($"Admin: {username} - {password}");
            
        //         return new Admin { Username = username, Password = password };
        //     }
        //     else
        //     {
        //         ShowMessage("Invalid input. Please try again.");
        //         return await CreateInitialAdminView();
        //     }
        // }

        public static async Task<string> ShowAdminMenu()
        {
            Console.WriteLine($"{red}Admin Menu{resetColor}");
            Console.WriteLine($"{blue}1.{magenta} Create Project {resetColor}");
            Console.WriteLine($"{blue}2.{magenta} Update Project{resetColor}");
            Console.WriteLine($"{blue}3.{magenta} Delete Project{resetColor}");
            Console.WriteLine($"{blue}4.{magenta} Create Task{resetColor}");
            Console.WriteLine($"{blue}5.{magenta} Update Task{resetColor}");
            Console.WriteLine($"{blue}6.{magenta} Delete Task{resetColor}");
            Console.WriteLine($"{blue}7.{magenta} Logout{resetColor}");
            Console.WriteLine($"{red}Enter your choice:{resetColor}");
            
            string userInput = GetUserInput();
            
            if (int.TryParse(userInput, out int parsedInput) && Utility.ValidateRange(parsedInput, 1, 7))
            {
                return userInput;
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid option.{resetColor}");
                return await ShowAdminMenu();
            }
        }

        public static Project GetProjectInput()
        {
            Console.WriteLine($"{green}Enter Project Name:{resetColor}");
            string projectName = GetUserInput();

            return new Project { Name = projectName };
        }

        public static void ShowProjectList(List<Project> projects)
        {
            Console.WriteLine($"{red}Available Projects:{resetColor}");
            foreach (var project in projects)
            {
                Console.WriteLine($"{project.Id}. {project.Name}");
            }
        }

        public static int GetProjectIdInput()
        {
            Console.WriteLine($"{green}Enter Project ID:{resetColor}");
            string projectIdInput = GetUserInput();

            if (int.TryParse(projectIdInput, out int projectId))
            {
                return projectId;
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid Project ID.{resetColor}");
                return GetProjectIdInput();
            }
        }

        public static ProjectTask GetTaskInput()
        {
            Console.WriteLine($"{green}Enter Task Description:{resetColor}");
            string taskDescription = GetUserInput();

            return new ProjectTask { Description = taskDescription };
        }

        public static int GetTaskIdInput()
        {
            Console.WriteLine($"{green}Enter Task ID:{resetColor}");
            string taskIdInput = GetUserInput();

            if (int.TryParse(taskIdInput, out int taskId))
            {
                return taskId;
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid Task ID.{resetColor}");
                return GetTaskIdInput();
            }
        }

        public static async Task<Project> CreateProjectView()
        {
            Console.WriteLine($"{green}Enter project name:{resetColor}");
            string projectName = GetUserInput();

            return new Project
            {
                Name = projectName
            };
        }

        public static async Task<Project> SelectProjectToUpdate(List<Project> projects)
        {
            Console.WriteLine($"{green}Select a project to update:{resetColor}");
            ShowProjectList(projects);

            string projectIdInput = GetUserInput();
            if (int.TryParse(projectIdInput, out int projectId))
            {
                Project selectedProject = projects.FirstOrDefault(p => p.Id == projectId);
                if (selectedProject != null)
                {
                    return selectedProject;
                }
                else
                {
                    ShowMessage($"{red}Invalid project ID.{resetColor}");
                    return await SelectProjectToUpdate(projects);
                }
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid Project ID.{resetColor}");
                return await SelectProjectToUpdate(projects);
            }
        }


        public static async Task<Project> UpdateProjectView(Project project)
        {
            Console.WriteLine($"{orange}Updating project: {project.Name}{resetColor}");
            Console.WriteLine($"{green}Enter new project name:{resetColor}");
            string newProjectName = GetUserInput();

            project.Name = newProjectName;
            return project;
        }

        public static async Task<Project> SelectProjectToDelete(List<Project> projects)
        {
            Console.WriteLine($"{green}Select a project to delete:{resetColor}");
            ShowProjectList(projects);

            string projectIdInput = GetUserInput();
            if (int.TryParse(projectIdInput, out int projectId))
            {
                Project selectedProject = projects.FirstOrDefault(p => p.Id == projectId);
                if (selectedProject != null)
                {
                    return selectedProject;
                }
                else
                {
                    ShowMessage($"{red}Invalid project ID.{resetColor}");
                    return await SelectProjectToDelete(projects);
                }
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid Project ID.{resetColor}");
                return await SelectProjectToDelete(projects);
            }
        }

        public static async Task<Tuple<int, string, bool>> CreateTaskView()
        {
            Console.WriteLine($"{green}Enter task description:{resetColor}");
            string description = GetUserInput();

            // Get other inputs (int projectId and bool isCompleted)
            int projectId = GetProjectIdInput();
            bool isCompleted = GetIsCompletedInput();

            return Tuple.Create(projectId, description, isCompleted);
        }

        private static bool GetIsCompletedInput()
        {
            Console.WriteLine($"{orange}Is the task completed? ({green}yes{resetColor}/{red}no{orange}):{resetColor}");
            string input = GetUserInput();

            if (input.ToLower() == "yes")
            {
                return true;
            }
            else if (input.ToLower() == "no")
            {
                return false;
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter {green}'yes'{red} or {orange}'no'{red}.{resetColor}");
                return GetIsCompletedInput();
            }
        }

        public static async Task<int> SelectUserForTask(List<User> users)
        {
            Console.WriteLine($"{green}Select a user for the task:{resetColor}");

            foreach (var user in users)
            {
                Console.WriteLine($"{orange}ID: {user.Id}, Username: {user.Username}{resetColor}");
            }

            int selectedUserId;
            do
            {
                Console.Write($"{blue}Enter the ID of the user: {resetColor}");
            }
            while (!int.TryParse(Console.ReadLine(), out selectedUserId) || users.All(user => user.Id != selectedUserId));

            return selectedUserId;
        }

        public static async Task<int> SelectTaskToUpdate(List<ProjectTask> tasks)
        {
            Console.WriteLine($"{green}Select a task to update:{resetColor}");
            ShowTaskList(tasks);

            string taskIdInput = await GetUserInputAsync();
            if (int.TryParse(taskIdInput, out int taskId))
            {
                return taskId;
            }
            else
            {
                ShowMessage($"{red}Invalid task ID.{resetColor}");
                return await SelectTaskToUpdate(tasks);
            }
        }

        private static async Task<string> GetUserInputAsync()
        {
            return await Console.In.ReadLineAsync();
        }


        public static async Task<Tuple<string, bool>> UpdateTaskView(ProjectTask task)
        {
            Console.WriteLine($"{green}Updating task: {task.Description}{resetColor}");
            Console.WriteLine($"{orange}Enter new task description:{resetColor}");
            string newDescription = GetUserInput();

            Console.WriteLine($"{orange}Enter {yellow}true{orange} if the task is completed, otherwise enter {red}false{orange}:{resetColor}");
            string isCompletedInput = GetUserInput();
            bool newIsCompleted;
            if (bool.TryParse(isCompletedInput, out newIsCompleted))
            {
                return Tuple.Create(newDescription, newIsCompleted);
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter true or false.{resetColor}");
                return await UpdateTaskView(task);
            }
        }


        public static async Task<ProjectTask> SelectTaskToDelete(List<ProjectTask> tasks)
        {
            Console.WriteLine($"{green}Select a task to delete:{resetColor}");
            ShowTaskList(tasks);

            string taskIdInput = GetUserInput();
            if (int.TryParse(taskIdInput, out int taskId))
            {
                ProjectTask selectedTask = tasks.FirstOrDefault(t => t.Id == taskId);
                if (selectedTask != null)
                {
                    return selectedTask;
                }
                else
                {
                    ShowMessage($"{red}Invalid task ID.{resetColor}");
                    return await SelectTaskToDelete(tasks);
                }
            }
            else
            {
                ShowMessage($"{red}Invalid input. Please enter a valid Task ID.{resetColor}");
                return await SelectTaskToDelete(tasks);
            }
        }


        private static void ShowTaskList(List<ProjectTask> tasks)
        {
            Console.WriteLine($"{green}Available Tasks:{resetColor}");
            foreach (var task in tasks)
            {
                Console.WriteLine($"{orange}{task.Id}. {lightBlue}{task.Description}{resetColor}");
            }
        }

        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static string GetUserInput()
        {
            return Console.ReadLine();
        }
    }
}

