namespace CARS2;
using Microsoft.EntityFrameworkCore;

public class CarDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public CarDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Cars;Integrated Security=True;Trust Server Certificate=True;");
    }
}
