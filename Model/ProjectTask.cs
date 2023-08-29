using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Console.Model
{
public class ProjectTask
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; } 
    public bool IsCompleted { get; set; }
    [ForeignKey("User")]
    public int? UserId { get; set; }
    public User User { get; set; } 

    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    [InverseProperty("Tasks")]
    public Project Project { get; set; }

    public void printTask()
    {
        Console.WriteLine($"Id: {Id}, Description: {Description}, IsCompleted: {IsCompleted}, UserId: {UserId}, ProjectId: {ProjectId}");
    }

}
}