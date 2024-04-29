using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Models;

namespace LoginContacts.Data
{
    public class LoginContactsContext : DbContext
    {
        public LoginContactsContext (DbContextOptions<LoginContactsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Contacts) // Configures the one-to-many relationship
                .WithOne(c => c.User)     // Specifies the foreign key property
                .HasForeignKey(c => c.UserId); // Explicitly defines the foreign key
        }

        public DbSet<Login.Models.User> User { get; set; }
        public DbSet<Login.Models.Contact> Contact { get; set; }
    }
}
