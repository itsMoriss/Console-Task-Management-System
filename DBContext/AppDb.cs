using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Task_Console.Model;

namespace Task_Console.DBContext
{
    public class AppDb: DbContext
    {
        public DbSet<User> users {get; set;}
        public DbSet<Admin> admins {get; set;}
        public DbSet<Project> projects {get; set;}
        public DbSet<ProjectTask> tasks {get; set;}

        public class AppConfig
        {   
            public string? Server { get; set; }
            public string? User { get; set; }
            public string? Password { get; set; }
            public string? Database { get; set; }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string json = File.ReadAllText("config.json");
            AppConfig? config = JsonSerializer.Deserialize<AppConfig>(json);
            string connectionString = $"Server={config.Server}; user={config.User}; password={config.Password}; Database={config.Database}; Trusted_Connection=True; TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
