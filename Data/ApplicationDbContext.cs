using Microsoft.EntityFrameworkCore;
using MVCEmployeeDemo.Models;
using System.Data;

namespace MVCEmployeeDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> employees { get; set; }
    }
}
