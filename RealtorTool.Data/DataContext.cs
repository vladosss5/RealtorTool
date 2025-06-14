using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbModels;
using RealtorTool.Core.DbModels.PersonModels;
using RealtorTool.Core.DbModels.Realty;

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
    
    /// <inheritdoc cref="Realty" />
    public DbSet<Realty> Realties { get; set; }
    
    /// <inheritdoc cref="Area" />
    public DbSet<Area> Areas { get; set; }
    
    /// <inheritdoc cref="Flat" />
    public DbSet<Flat> Flats { get; set; }
    
    /// <inheritdoc cref="House" />
    public DbSet<House> Houses { get; set; }
    
    /// <inheritdoc cref="PrivateHome" />
    public DbSet<PrivateHome> PrivateHomes { get; set; }
    
    /// <inheritdoc cref="Address" />
    public DbSet<Address> Addresses { get; set; }
    
    /// <inheritdoc cref="Photo" />
    public DbSet<Photo> Photos { get; set; }

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
            .WithOne(a => a.Person)
            .HasForeignKey<AuthorizationData>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.RealtorDetails)
            .WithOne()
            .HasForeignKey<RealtorDetails>(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClientTags>()
            .HasOne(ct => ct.Client)
            .WithMany()
            .HasForeignKey(ct => ct.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Area>()
            .HasOne(a => a.Address)
            .WithMany()
            .HasForeignKey(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Flat>()
            .HasOne(f => f.House)
            .WithMany()
            .HasForeignKey(f => f.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PrivateHome>()
            .HasOne(ph => ph.Area)
            .WithMany()
            .HasForeignKey(ph => ph.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Realty>()
            .HasMany(r => r.Photos)
            .WithOne()
            .HasForeignKey(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Realty>()
            .HasOne(r => r.Application)
            .WithMany()
            .HasForeignKey(r => r.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Flat>()
            .HasOne(f => f.BathroomType)
            .WithMany()
            .HasForeignKey(f => f.BathroomTypeId);

        modelBuilder.Entity<Flat>()
            .HasOne(f => f.Repair)
            .WithMany()
            .HasForeignKey(f => f.RepairId);

        modelBuilder.Entity<Flat>()
            .HasOne(f => f.UsedType)
            .WithMany()
            .HasForeignKey(f => f.UsedTypeId);

        modelBuilder.Entity<House>()
            .HasOne(h => h.СonstructionType)
            .WithMany()
            .HasForeignKey(h => h.СonstructionTypeId);

        modelBuilder.Entity<Realty>()
            .HasOne(r => r.Type)
            .WithMany()
            .HasForeignKey(r => r.TypeId);
    }
}