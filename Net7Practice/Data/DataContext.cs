
namespace Net7Practice.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }
        public DbSet<Characteristic> characteristics { get; set; }  
        public DbSet<Users> Users { get; set; } 
    }
}
