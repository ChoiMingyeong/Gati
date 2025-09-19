using Microsoft.EntityFrameworkCore;
using ServerLib.Model;

namespace ServerLib
{
	public class AuthContext : DbContext
	{
		public DbSet<auth> Auth { get; set; }

		public AuthContext(DbContextOptions<AuthContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<auth>(entity =>
			{
				entity.ToTable("auth");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).HasColumnName("Id");
				entity.Property(e => e.UserId).HasColumnName("UserId").IsRequired();
				entity.Property(e => e.UserEmail).HasColumnName("UserEmail").HasMaxLength(255).IsRequired();
				entity.HasIndex(e => e.UserId).IsUnique();
				entity.HasIndex(e => e.UserEmail).IsUnique();
			});
		}
	}
}


