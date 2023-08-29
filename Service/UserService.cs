using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Task_Console.DBContext;
using Task_Console.Model;
using Task_Console.Utilities;
using Task_Console.Views;

namespace Task_Console.Service
{
    public class UserService
    {
        private readonly AppDb _context;
        private User loginUser;

        public UserService()
        {
            _context = new AppDb();
        }

        public List<User> GetAllUsers()
        {
            return _context.users.ToList();
        }

        public bool RegisterUser(User user)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(user);
                if (!Validator.TryValidateObject(user, validationContext, validationResults))
                {
                    AppView.ShowMessage("Invalid user");
                    return false;
                }
                if (_context.users.Any(u => u.Username == user.Username))
                {
                    AppView.ShowMessage("Username already exists");
                    return false;
                }
                user.Password = Utility.EncodePassword(user.Password);
            
                _context.users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // throw new Exception("An error occurred while hashing the password.", ex);
                AppView.ShowMessage(ex.Message);
                return false;
            }
        }
        public User LoginUser(string username, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                AppView.ShowMessage("User not found");
                return null;
            }
            if (!Utility.VerifyPassword(password, user.Password))
            {
                AppView.ShowMessage("Incorrect password");
                return null;
            }

            loginUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Tasks = GetUserTasks(user.Id)
            };

            return loginUser;
        }


        public List<ProjectTask> GetUserTasks(int userId)
        {
            var tasks = _context.tasks.Where(t => t.UserId == userId).ToList();
            return tasks;
        }

        public List<ProjectTask> GetUserTasks()
        {
            var tasks = _context.tasks.Where(t => t.UserId == loginUser.Id).ToList();
            return tasks;
        }

        public List<Project> GetUserProjects()
        {
            var projects = _context.projects
                .Where(p => p.Tasks.Any(t => t.UserId == loginUser.Id))
                .Include(p => p.Tasks)
                .ToList();
            return projects;
        }

        // show single project details
        public Project GetUserProject(int projectId)
        {
            var project = _context.projects
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == projectId);
            return project;
        }

        public ProjectTask SetTaskDone(int taskId)
        {
            var task = _context.tasks.FirstOrDefault(t => t.Id == taskId);
            ProjectTask projectTask = new ProjectTask
            {
                Id = task.Id,
                Description = task.Description,
                IsCompleted = true,
                UserId = task.UserId,
                ProjectId = task.ProjectId
            };
            _context.SaveChanges();
            return projectTask;
        }
        public ProjectTask UpdateTask(ProjectTask task)
        {
            _context.tasks.Update(task);
            _context.SaveChanges();
            return task;
        }
        public User GetLoginUser()
        {
            return loginUser;
        }
    }
}
