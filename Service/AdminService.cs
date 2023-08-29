using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.DBContext;
using Task_Console.Model;
using Task_Console.Utilities;
using Task_Console.Views;
using System.ComponentModel.DataAnnotations;

namespace Task_Console.Service
{
    public class AdminService
    {
        private readonly AppDb _context;
        private Admin loginAdmin;

        public AdminService()
        {
            _context = new AppDb();
        }

        public bool RegisterAdmin(Admin admin)
        {
            // Check if the admin with the same username already exists
            if (_context.admins.Any(a => a.Username == admin.Username))
            {
                AppView.ShowMessage("Admin with the same username already exists.");
                return false;
            }

            // Hash the admin's password and save the hashed password
            admin.Password = Utility.EncodePassword(admin.Password);
            _context.admins.Add(admin);
            _context.SaveChanges();
            return true;
        }


        public Admin LoginAdmin(string username, string password)
        {
            var admin = _context.admins.FirstOrDefault(a => a.Username == username);
            if (admin == null)
            {
                AppView.ShowMessage("Admin not found");
                return null;
            }
            if (!Utility.VerifyPassword(password, admin.Password))
            {
                AppView.ShowMessage("Incorrect password");
                return null;
            }

            loginAdmin = admin;
            return loginAdmin;
        }

        public bool CreateProject(Project project)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(project);
                if (!Validator.TryValidateObject(project, validationContext, validationResults))
                {
                    AppView.ShowMessage("Invalid project");
                    return false;
                }

                project.AdminId = loginAdmin.Id;
                _context.projects.Add(project);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AppView.ShowMessage(ex.Message);
                return false;
            }
        }

        public bool UpdateProject(Project project)
        {
            var existingProject = _context.projects.FirstOrDefault(p => p.Id == project.Id && p.AdminId == loginAdmin.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteProject(int projectId)
        {
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId && p.AdminId == loginAdmin.Id);
            if (project != null)
            {
                _context.projects.Remove(project);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CreateTask(int projectId, ProjectTask task)
        {
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId && p.AdminId == loginAdmin.Id);
            if (project != null)
            {
                task.ProjectId = project.Id;
                _context.tasks.Add(task);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateTask(ProjectTask task)
        {
            var existingTask = _context.tasks.FirstOrDefault(t => t.Id == task.Id && t.Project.AdminId == loginAdmin.Id);
            if (existingTask != null)
            {
                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteTask(int taskId)
        {
            var task = _context.tasks.FirstOrDefault(t => t.Id == taskId && t.Project.AdminId == loginAdmin.Id);
            if (task != null)
            {
                _context.tasks.Remove(task);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ProjectTask> GetProjectTasks(int projectId)
        {
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId && p.AdminId == loginAdmin.Id);
            if (project != null)
            {
                return project.Tasks.ToList();
            }
            return new List<ProjectTask>();
        }
    }
}
