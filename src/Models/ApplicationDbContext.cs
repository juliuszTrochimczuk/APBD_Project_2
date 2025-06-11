using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceEmployee> DevicesEmployees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Person> Person { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin"},    
                new Role { Id = 1, Name = "User"}    
            );
            modelBuilder.Entity<Account>().HasIndex(a => a.Username).IsUnique();
        }
    }
}
