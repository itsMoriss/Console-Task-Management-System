using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Views;
using Task_Console.Service;
using Task_Console.Model;

namespace Task_Console.Controllers
{
    public class UserController
    {
        // user router
        public static readonly UserService _userService = new UserService();
        
        public static async Task UserBaseRouter(string Option)
        {
            switch (Option)
            {
                case "1":
                    await UserRegister();
                    break;
                case "2":
                    await LoginUser();
                    break;
                case "4":
                    await ExitApp();
                    break;
                default:
                    AppView.ShowMessage("Invalid option");
                    await AppView.InitApp();
                    break;
            }
        }

        public static async Task UserRegister()
        {
            Tuple<string, string> userInput = await UserView.RegisterUserView();
            User newUser = new User(){
                Username= userInput.Item1,
                Password= userInput.Item2,
            };
            if(_userService.RegisterUser(newUser))
            {
                await LoginUser();
            }
            else{
                await AppView.InitApp();
            }
        }
        public static async Task LoginUser()
        {
            Tuple<string, string> userInput = await UserView.LoginUserView();
            string Username= userInput.Item1;
            string Password= userInput.Item2;
            if(_userService.LoginUser(Username, Password) != null)
            {
                await ShowUserMenuGetOption();
            }else{
                AppView.ShowMessage("Invalid username or password");
                await AppView.InitApp();
            }
        }

        // Exit the application
        public static async Task ExitApp(){
            AppView.ShowMessage("Thank you for using our app");
            Environment.Exit(0);
		}

        public static async Task ShowUserMenuGetOption()
        {
            string option = await UserView.ShowUserMenu();
            await UserMenuRouter(option);
        }

        public static async Task UserMenuRouter(string option)
        {
            switch(option)
            {
                case "1":
                    await ShowUserProjectRoute();
                    break;
                case "2":
                    await ShowUserSingleProjectRoute();
                    break;
                case "3":
                    await ShowUserUndoneTasksRoute();
                    break;
                case "4":
                    await ShowUserSingleTaskRoute();
                    break;
                case "5":
                    await ExitApp();
                    break;
                default:
                    AppView.ShowMessage("Invalid option. Please try again.");
                    await UserView.ShowUserMenu();
                    break;
            }
        }

        public static async Task ShowUserProjectRoute()
        {
            List<Project> userProjects = _userService.GetUserProjects();
            await UserView.ShowUserProject(userProjects);
             await ShowUserMenuGetOption();
        }

        public static async Task ShowUserSingleProjectRoute()
        {
            AppView.ShowMessage("Choose a project");
            List<Project> userProjects = _userService.GetUserProjects();
            await UserView.ShowUserProject(userProjects);

            AppView.ShowMessage("Please enter the project ID");
            string projectIdInput = AppView.GetUserInput();
            
            if (int.TryParse(projectIdInput, out int projectId) && 
                userProjects.Any(project => project.Id == projectId))
            {
                Project selectedProject = _userService.GetUserProject(projectId);
                List<Project> selectedProjectList = new List<Project> { selectedProject };
                await UserView.ShowUserProject(selectedProjectList);
            }
            else
            {
                AppView.ShowMessage("Invalid project ID. Please try again.");
                await ShowUserMenuGetOption();
            }
        }

        public static async Task ShowUserUndoneTasksRoute()
        {
            User user = _userService.GetLoginUser();
            Console.WriteLine(user.Tasks.Count);
            if (user.Tasks.Count > 0){
                user.PrintUserUndoneTasks();
               string userInput = await UserView.CheckTaskDone();
                if (int.TryParse(userInput, out int taskId) && 
                    user.Tasks.Any(task => task.Id == taskId))
                {
                    ProjectTask selectedTask = user.Tasks.FirstOrDefault(task => task.Id == taskId);
                    selectedTask.IsCompleted = true;
                    if (selectedTask != null){
                        ProjectTask userTask = _userService.UpdateTask(selectedTask);
                        if (userTask.IsCompleted == true){
                            AppView.ShowMessage($"Task {userTask.Id} is done.");
                        await ShowUserMenuGetOption();
                        }
                    }
                    else {
                        AppView.ShowMessage("Invalid task ID. Please try again.");
                        await ShowUserUndoneTasksRoute();
                    }
                }
                else
                {
                    AppView.ShowMessage("Invalid task ID. Please try again.");
                    await ShowUserUndoneTasksRoute();
                }
            }
            else
            {
                AppView.ShowMessage("There are no undone tasks");
                await ShowUserMenuGetOption();
            }

        }

        public static async Task ShowUserSingleTaskRoute()
        {
            
            AppView.ShowMessage("Choose a task");
            List<ProjectTask> userTasks = _userService.GetUserTasks();
            await UserView.ShowUserTasksView(userTasks);

            AppView.ShowMessage("Please enter the task ID");
            string taskIdInput = AppView.GetUserInput();
            
            if (int.TryParse(taskIdInput, out int taskId) && 
                userTasks.Any(task => task.Id == taskId))
            {
                List<ProjectTask> selectedTask = userTasks.Where(task => task.Id == taskId).ToList();
                await UserView.ShowUserTasksView(selectedTask);
                await ShowUserMenuGetOption();
            }
            else
            {
                AppView.ShowMessage("Invalid task ID. Please try again.");
                await ShowUserSingleTaskRoute();
            }
        }

    }
}