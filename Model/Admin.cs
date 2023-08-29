using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Console.Model
{
public class Admin
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } 
    public string Password { get; set; }
     public ICollection<Project> Projects { get; set; } = new List<Project>();
}
}