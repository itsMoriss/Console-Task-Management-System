using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.DBContext;
using Task_Console.Model;
using Task_Console.Utilities;
using Task_Console.Views;
using System.ComponentModel.DataAnnotations;
using Task_Console.Controllers;

namespace Task_Console.Service
{
    public class ProjectService
    {
        private readonly AppDb _context;
        private readonly Admin _admin;

        public ProjectService(Admin admin)
        {
            _context = new AppDb();
            _admin = admin;
        }

        public List<Project> GetAllProjects()
        {
            return _context.projects.Where(p => p.AdminId == _admin.Id).ToList();
        }

        public List<Project> GetAdminProjects(int adminId)
        {
            return _context.projects.Where(p => p.AdminId == adminId).ToList();
        }

        public Project GetProjectById(int projectId, int adminId)
        {
            return _context.projects.FirstOrDefault(p => p.Id == projectId && p.AdminId == adminId);
        }

        public bool UpdateProject(Project project)
        {
            var existingProject = _context.projects.FirstOrDefault(p => p.Id == project.Id && p.AdminId == _admin.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name; // Update other properties as needed
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteProject(int projectId, int adminId)
        {
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId && p.AdminId == adminId);
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
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId);
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
            var existingTask = _context.tasks.FirstOrDefault(t => t.Id == task.Id);
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
            var task = _context.tasks.FirstOrDefault(t => t.Id == taskId);
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
            var project = _context.projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                return project.Tasks.ToList();
            }
            return new List<ProjectTask>();
        }
    }
}
