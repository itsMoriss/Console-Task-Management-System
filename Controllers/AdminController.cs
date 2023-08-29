using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Service;
using Task_Console.Model;
using Task_Console.Utilities;
using Task_Console.Views;

namespace Task_Console.Controllers
{
    public class AdminController
    {
        private static readonly AdminService _adminService = new AdminService();
        private static ProjectService _projectService;
        private static readonly TaskService _taskService = new TaskService();
        private static readonly UserService _userService = new UserService();

        private static Admin loginAdmin;

        public static async Task AdminBaseRouter(int option)
        {
            switch (option)
            {
                case 3:
                await AdminLogin();
                break;
                case 4:
                await RegisterAdmin();
                break;
                case 5:
                await ExitApp();
                break;
                default:
                AppView.ShowMessage("Invalid option");
                await AppView.InitApp();
                break;
            }
        }

        public static async Task RegisterAdmin()
        {
            Tuple<string, string> userInput = await AdminView.RegisterAdminView();
            string Username = userInput.Item1;
            string Password = userInput.Item2;

            Admin newAdmin = new Admin
            {
                Username = Username,
                Password = Utility.EncodePassword(Password)
            };

            if (_adminService.RegisterAdmin(newAdmin))
            {
                AppView.ShowMessage("Admin registered successfully!");
                loginAdmin = newAdmin; // Automatically log in the registered admin
                await ShowAdminMenuGetOption(loginAdmin.Id);
            }
            else
            {
                AppView.ShowMessage("Error registering admin.");
            }
        }


        public static async Task AdminLogin()
        {
            Tuple<string, string> userInput = await AdminView.AdminLoginView();
            string Username = userInput.Item1;
            string Password = userInput.Item2;

            Admin admin = _adminService.LoginAdmin(Username, Password);
            if (admin != null)
            {
                loginAdmin = admin;
                _projectService = new ProjectService(loginAdmin);
                await ShowAdminMenuGetOption(loginAdmin.Id); // Show admin menu after successful login
            }
            else
            {
                AppView.ShowMessage("Invalid username or password");
                await AppView.InitApp();
            }
        }


        public static async Task ShowAdminMenuGetOption(int adminId)
        {
            string option = await AdminView.ShowAdminMenu();
            await AdminMenuRouter(option, adminId);
        }

        public static async Task AdminMenuRouter(string option, int adminId)
        {
            switch (option)
            {
                case "1":
                    await CreateProject(adminId);
                    break;
                case "2":
                    await UpdateProject();
                    break;
                case "3":
                    await DeleteProject(adminId);
                    break;
                case "4":
                    await CreateTask();
                    break;
                case "5":
                    await UpdateTask();
                    break;
                case "6":
                    await DeleteTask();
                    break;
                case "7":
                    await ExitApp();
                    break;
                default:
                    AppView.ShowMessage("Invalid option. Please try again.");
                    await ShowAdminMenuGetOption(adminId);
                    break;
            }
        }

        public static async Task CreateProject(int adminId)
        {
            Project projectInput = await AdminView.CreateProjectView();
            ProjectService projectService = new ProjectService(loginAdmin);
            string projectName = projectInput.Name;

            Project project = new Project
            {
                Name = projectName
            };

            if (_adminService.CreateProject(project))
            {
                AppView.ShowMessage("Project created successfully!");
            }
            else
            {
                AppView.ShowMessage("Error creating project.");
            }

            await ShowAdminMenuGetOption(adminId);
        }

        public static async Task UpdateProject()
        {
            List<Project> projects = _projectService.GetAllProjects();
            if (projects.Count == 0)
            {
                AppView.ShowMessage("No projects available to update.");
                return;
            }

            Project selectedProject = await AdminView.SelectProjectToUpdate(projects);
            if (selectedProject != null)
            {
                Project projectInput = await AdminView.UpdateProjectView(selectedProject);
                string newName = projectInput.Name;

                selectedProject.Name = newName;
                if (_projectService.UpdateProject(selectedProject))
                {
                    AppView.ShowMessage("Project updated successfully!");
                }
                else
                {
                    AppView.ShowMessage("Error updating project.");
                }
            }
        }

        public static async Task DeleteProject(int adminId)
        {
            List<Project> projects = _projectService.GetAllProjects();
            if (projects.Count == 0)
            {
                AppView.ShowMessage("No projects available to delete.");
                return;
            }

            Project selectedProject = await AdminView.SelectProjectToDelete(projects);
            if (selectedProject != null)
            {
                if (_projectService.DeleteProject(selectedProject.Id, adminId))
                {
                    AppView.ShowMessage("Project deleted successfully!");
                }
                else
                {
                    AppView.ShowMessage("Error deleting project.");
                }
            }
        }

        public static async Task CreateTask()
        {
            List<Project> projects = _projectService.GetAllProjects();
            List<User> users = _userService.GetAllUsers(); // Get all users from the service

            if (projects.Count == 0)
            {
                AppView.ShowMessage("No projects available to create task under.");
                return;
            }

            Tuple<int, string, bool> userInput = await AdminView.CreateTaskView();
            int projectId = userInput.Item1;
            string taskDescription = userInput.Item2;
            bool isCompleted = userInput.Item3;

            int userId = await AdminView.SelectUserForTask(users); // Ensure a valid user is selected

            Project project = projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                ProjectTask newTask = new ProjectTask
                {
                    Description = taskDescription,
                    IsCompleted = isCompleted,
                    UserId = userId, // Assign the selected user to the task
                    ProjectId = projectId
                };

                if (_projectService.CreateTask(projectId, newTask))
                {
                    AppView.ShowMessage("Task created successfully!");
                }
                else
                {
                    AppView.ShowMessage("Error creating task.");
                }
            }
            else
            {
                AppView.ShowMessage("Invalid project selected.");
            }
        }

        public static async Task UpdateTask()
        {
            List<ProjectTask> tasks = _taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                AppView.ShowMessage("No tasks available to update.");
                return;
            }

            int selectedTaskId = await AdminView.SelectTaskToUpdate(tasks);
            ProjectTask selectedTask = tasks.FirstOrDefault(t => t.Id == selectedTaskId);


            if (selectedTask != null)
            {
                Tuple<string, bool> userInput = await AdminView.UpdateTaskView(selectedTask);
                string newDescription = userInput.Item1;
                bool newIsCompleted = userInput.Item2;

                selectedTask.Description = newDescription;
                selectedTask.IsCompleted = newIsCompleted;
        
                if (_taskService.UpdateTask(selectedTask))
                {
                    AppView.ShowMessage("Task updated successfully!");
                }
                else
                {
                    AppView.ShowMessage("Error updating task.");
                }
            }
        }

        public static async Task DeleteTask()
        {
            List<ProjectTask> tasks = _taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                AppView.ShowMessage("No tasks available to delete.");
                return;
            }

            ProjectTask selectedTask = await AdminView.SelectTaskToDelete(tasks);
            if (selectedTask != null)
            {
                if (_taskService.DeleteTask(selectedTask.Id))
                {
                    AppView.ShowMessage("Task deleted successfully!");
                }
                else
                {
                    AppView.ShowMessage("Error deleting task.");
                }
            }
        }

        public static async Task ExitApp()
        {
            AppView.ShowMessage("Thank you for using our app");
            Environment.Exit(0);
        }
    }
}

