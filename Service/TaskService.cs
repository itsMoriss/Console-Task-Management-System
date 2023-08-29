using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Console.Model;
using Task_Console.Views;
using Task_Console.DBContext;

namespace Task_Console.Service
{
    public class TaskService
    {
        private readonly AppDb _context;

        public TaskService()
        {
            _context = new AppDb();
        }

        public List<ProjectTask> GetAllTasks()
        {
            try
            {
                return _context.tasks.ToList();
            }
            catch (Exception ex)
            {
                AppView.ShowMessage("Error getting tasks: " + ex.Message);
                return new List<ProjectTask>();
            }
        }

        public bool CreateTask(ProjectTask task)
        {
            try
            {
                _context.tasks.Add(task);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AppView.ShowMessage("Error creating task: " + ex.Message);
                return false;
            }
        }

        public bool UpdateTask(ProjectTask task)
        {
            try
            {
                _context.tasks.Update(task);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AppView.ShowMessage("Error updating task: " + ex.Message);
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                var taskToDelete = _context.tasks.FirstOrDefault(t => t.Id == taskId);
                if (taskToDelete != null)
                {
                    _context.tasks.Remove(taskToDelete);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    AppView.ShowMessage("Task not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppView.ShowMessage("Error deleting task: " + ex.Message);
                return false;
            }
        }

    }

}