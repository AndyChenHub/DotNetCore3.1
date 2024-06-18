using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Setup
{
    public static class DbInitialiser
    {
        public static void Initialise(UserDbContext context)
        {
            // Ensure database is created and migrated
            context.Database.Migrate();

            foreach (var entity in context.ChangeTracker.Entries().ToList())
            {
                context.Entry(entity.Entity).State = EntityState.Detached;
            }
            // Check if data already exists
            if (context.Users.Any())
            {
                return;
            }


            string[] firstNames = { "John", "Jane", "Michael", "Emily", "William", "Olivia", "James", "Sophia", "Alexander", "Emma", "Daniel", "Isabella", "David", "Mia", "Benjamin", "Charlotte", "Samuel", "Amelia", "Ethan", "Ella" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson" };

            // Generate Users
            var users = new User[firstNames.Length];
            for (int i = 0; i < firstNames.Length; i++)
            {
                users[i] = new User
                {
                    UserName = $"{firstNames[i]}_{lastNames[i]}",
                    Email = $"{firstNames[i]}_{lastNames[i]}@hotmail.com",
                    Alias = $"{i + 1}",
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    UserType = i < 7 ? UserType.Manager : UserType.Client
                };
            }

            // Generate Managers
            var managers = new Manager[7];
            for (int i = 0; i < 7; i++)
            {
                managers[i] = new Manager
                {
                    ManagerId = i + 1,
                    Position = i % 2 == 0 ? Position.Senior : Position.Junior,
                    User = users[i]
                };
            }

            // Generate Clients 
            var clients = new Client[13];
            for (int i = 0; i < 13; i++)
            {
                clients[i] = new Client
                {
                    Level = i + 1,
                    User = users[i + 7],
                    Manager = managers[i % 7]
                };
            }

            // Add generated data to context and save changes
            context.AddRange(users);
            context.AddRange(managers);
            context.AddRange(clients);
            context.SaveChanges();
        }
    }
}
