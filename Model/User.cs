using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Task_Console.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
        public ICollection<ProjectTask>? Tasks { get; set; } = new List<ProjectTask>();


        public void PrintUserTasks()
        {
            Console.WriteLine($"User {Username} has the following tasks:");
            Console.WriteLine("ID\tProject\t\tDescription\t\tStatus");

            foreach (var task in Tasks)
            {
                Console.WriteLine($"{task.Id}\t{task.Project}\t\t{task.Description}\t\t{task.IsCompleted}");
            }
        }

        public void PrintUserUndoneTasks()
        {
            if (Tasks.Any(task => !task.IsCompleted))
            {
                Console.WriteLine($"User {Username} has the following undone tasks:");
                Console.WriteLine("ID\tProject\t\tDescription");

                foreach (ProjectTask task in Tasks)
                {
                    if (!task.IsCompleted)
                    {
                        task.printTask();
                    }
                }
            }
            else
            {
                Console.WriteLine($"User {Username} has no undone tasks.");
            }
        }
    }

}