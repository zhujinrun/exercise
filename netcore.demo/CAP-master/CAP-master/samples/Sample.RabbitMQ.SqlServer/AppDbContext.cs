using Microsoft.EntityFrameworkCore;

namespace Sample.RabbitMQ.SqlServer
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Id:{Id}";
        }
    } 

    public class AppDbContext : DbContext
    {
        public const string ConnectionString = "server=47.94.198.11,1433;database=BulkTestDB;uid=sa;pwd=Root1230;";

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
