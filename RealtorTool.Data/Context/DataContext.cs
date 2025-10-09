using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Data.Context;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=RT;");

    // Основные DbSet
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Dictionary> Dictionaries { get; set; } = null!;
    public DbSet<DictionaryValue> DictionaryValues { get; set; } = null!;
    
    // Недвижимость и её наследники
    public DbSet<Realty> Realties { get; set; } = null!;
    public DbSet<Apartment> Apartments { get; set; } = null!;
    public DbSet<PrivateHouse> PrivateHouses { get; set; } = null!;
    public DbSet<Area> Areas { get; set; } = null!;
    
    // Предложения и сделки
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Deal> Deals { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table Per Type (TPT) для наследования Realty
        modelBuilder.Entity<Realty>().ToTable("Realties");
        modelBuilder.Entity<Apartment>().ToTable("Apartments");
        modelBuilder.Entity<PrivateHouse>().ToTable("PrivateHouses");
        modelBuilder.Entity<Area>().ToTable("Areas");

        // Настройка связей для Realty
        modelBuilder.Entity<Realty>()
            .HasOne(r => r.Address)
            .WithMany()
            .HasForeignKey(r => r.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Realty>()
            .HasOne(r => r.ParentRealty)
            .WithMany(r => r.ChildRealties)
            .HasForeignKey(r => r.ParentRealtyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связей для DictionaryValue
        modelBuilder.Entity<DictionaryValue>()
            .HasOne(dv => dv.Dictionary)
            .WithMany()
            .HasForeignKey(dv => dv.DictionaryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связей для Listing
        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Realty)
            .WithMany(r => r.Listings)
            .HasForeignKey(l => l.RealtyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Owner)
            .WithMany()
            .HasForeignKey(l => l.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.ResponsibleEmployee)
            .WithMany()
            .HasForeignKey(l => l.ResponsibleEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Currency)
            .WithMany()
            .HasForeignKey(l => l.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.ListingType)
            .WithMany()
            .HasForeignKey(l => l.ListingTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Status)
            .WithMany()
            .HasForeignKey(l => l.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связей для Deal
        modelBuilder.Entity<Deal>()
            .HasOne(d => d.Listing)
            .WithMany()
            .HasForeignKey(d => d.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deal>()
            .HasOne(d => d.Buyer)
            .WithMany()
            .HasForeignKey(d => d.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deal>()
            .HasOne(d => d.Employee)
            .WithMany()
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deal>()
            .HasOne(d => d.DealType)
            .WithMany()
            .HasForeignKey(d => d.DealTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deal>()
            .HasOne(d => d.Status)
            .WithMany()
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связей для Apartment
        modelBuilder.Entity<Apartment>()
            .HasOne(a => a.RenovationType)
            .WithMany()
            .HasForeignKey(a => a.RenovationTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Apartment>()
            .HasOne(a => a.BathroomType)
            .WithMany()
            .HasForeignKey(a => a.BathroomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связей для PrivateHouse
        modelBuilder.Entity<PrivateHouse>()
            .HasOne(ph => ph.HeatingType)
            .WithMany()
            .HasForeignKey(ph => ph.HeatingTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PrivateHouse>()
            .HasOne(ph => ph.ConstructionMaterial)
            .WithMany()
            .HasForeignKey(ph => ph.ConstructionMaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        // Уникальные индексы
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Login)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Phone)
            .IsUnique();

        modelBuilder.Entity<Dictionary>()
            .HasIndex(d => d.Type)
            .IsUnique();
    }
}