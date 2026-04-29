using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Description)
            .IsRequired();
    }
}