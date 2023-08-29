using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Task_Console.Model
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public Admin Admin { get; set; }

        public void PrintProject()
        {
            Console.WriteLine($"Project ID: {Id}");
            Console.WriteLine($"Project Name: {Name}");
            Console.WriteLine($"Admin ID: {AdminId}");
            Console.WriteLine("Tasks:");
            Console.WriteLine("Task ID\tTask Title\tTask Description\tTask Status");

            foreach (var task in Tasks)
            {
                // Console.WriteLine($"{task.Id}\t{task.Title}\t{task.Description}\t{task.Status}");
                task.printTask();
            }
        }
    }
}