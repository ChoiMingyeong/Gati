using Microsoft.EntityFrameworkCore;
using ServerLib.Model;

namespace ServerLib
{
    public class UserContext : DbContext
    {
        public DbSet<account> Account { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<account>(entity =>
			{
				entity.ToTable("account");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).HasColumnName("Id");
				entity.Property(e => e.UserName).HasColumnName("UserName").HasMaxLength(50).IsRequired();
                entity.Property(e => e.LoginTime).HasColumnName("LoginTime");
                entity.Property(e => e.Exp).HasColumnName("Exp");
			});
        }
    }
}

