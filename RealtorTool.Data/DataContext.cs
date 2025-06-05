using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbModels;
using RealtorTool.Core.DbModels.PersonModels;

namespace RealtorTool.Data;

/// <summary>
/// Контекст БД.
/// </summary>
public partial class DataContext : DbContext
{
    /// <inheritdoc cref="Person" />
    public DbSet<Person> Persons { get; set; }
    
    /// <inheritdoc cref="AuthorizationData" />
    public DbSet<AuthorizationData> AuthorizationData { get; set; }
    
    /// <inheritdoc cref="Dictionary" />
    public DbSet<Dictionary> Dictionaries => Set<Dictionary>();

    /// <inheritdoc cref="DictionaryValue" />
    public DbSet<DictionaryValue> DictionaryValues => Set<DictionaryValue>();

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
        modelBuilder.Entity<Person>()
            .HasOne(p => p.AuthorizationData)
            .WithOne()
            .HasForeignKey<AuthorizationData>(ad => ad.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Person>()
            .HasOne(p => p.RealtorDetails)
            .WithOne()
            .HasForeignKey<RealtorDetails>(ad => ad.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Role)
            .WithMany()
            .HasForeignKey("RoleId")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Dictionary>()
            .HasMany(d => d.DictionaryValues)
            .WithOne(dv => dv.Dictionary)
            .HasForeignKey("DictionaryId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClientTags>()
            .HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey("clientTag_client")
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ClientTags>()
            .HasOne(x => x.Tag)
            .WithMany()
            .HasForeignKey("clientTag_tag")
            .OnDelete(DeleteBehavior.Cascade);
    }
}