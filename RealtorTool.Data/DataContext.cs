using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbModels;
using RealtorTool.Core.DbModels.Person;

namespace RealtorTool.Data;

/// <summary>
/// Контекст БД.
/// </summary>
public partial class DataContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    
    public DbSet<AuthorizationData> AuthorizationData { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Конструктор для миграций.
    /// </summary>
    public DataContext() { }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="options"></param>
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=Realtor;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.FName).IsRequired();
            entity.Property(p => p.SName).IsRequired();
            
            entity.HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey("RoleId")
                .IsRequired();
            
            entity.HasOne(p => p.AuthorizationData)
                .WithOne()
                .HasForeignKey<AuthorizationData>(ad => ad.Id)
                .IsRequired();
        });
        
        modelBuilder.Entity<AuthorizationData>(entity =>
        {
            entity.HasKey(ad => ad.Id);
            entity.Property(ad => ad.Login).IsRequired();
            entity.Property(ad => ad.Password).IsRequired();
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}