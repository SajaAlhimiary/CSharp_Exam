using Microsoft.EntityFrameworkCore;

namespace CSharpBeltExamSajaAlhimiary.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Enthusiast> Enthusiasts { get; set; }
    }
}