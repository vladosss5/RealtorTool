using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.RealtyModels;
using RealtorTool.Core.DbEntities.Views;

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

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Realty> Realties { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<PrivateHouse> PrivateHouses { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<ClientRequest> ClientRequests { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<DealParticipant> DealParticipants { get; set; }
    public DbSet<Dictionary> Dictionaries { get; set; }
    public DbSet<DictionaryValue> DictionaryValues { get; set; }
    public DbSet<Photo> Photos { get; set; }
    
    
    public DbSet<PotentialMatch> PotentialMatches { get; set; } = null!;

    /// <summary>
    /// Конфигурация модели данных
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация наследования Realty (TPT - Table Per Type)
        modelBuilder.Entity<Realty>().ToTable("Realties");
        modelBuilder.Entity<Apartment>().ToTable("Apartments");
        modelBuilder.Entity<PrivateHouse>().ToTable("PrivateHouses");
        modelBuilder.Entity<Area>().ToTable("Areas");

        // Конфигурация Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Street).HasMaxLength(200).IsRequired();
            entity.Property(e => e.HouseNumber).HasMaxLength(20);
            entity.Property(e => e.BuildingNumber).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
        });

        // Конфигурация Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.PassportSeries).HasMaxLength(10);
            entity.Property(e => e.PassportNumber).HasMaxLength(20);
            entity.Property(e => e.RegistrationAddress).HasMaxLength(500);

            // Связи
            entity.HasOne(e => e.Photo)
                  .WithOne()
                  .HasForeignKey<Client>(e => e.PhotoId)
                  .IsRequired(false);
        });

        // Конфигурация Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted && !e.Fired);
            
            entity.Property(e => e.Login).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.LastAuthentication).IsRequired(false);

            // Связи
            entity.HasOne(e => e.Role)
                  .WithMany()
                  .HasForeignKey(e => e.RoleId)
                  .IsRequired(false);

            entity.HasOne(e => e.Photo)
                  .WithOne()
                  .HasForeignKey<Employee>(e => e.PhotoId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Конфигурация Realty
        modelBuilder.Entity<Realty>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.AddressId).IsRequired();
            entity.Property(e => e.ParentRealtyId).IsRequired(false);
            entity.Property(e => e.RealtyType).IsRequired();

            // Связи
            entity.HasOne(e => e.Address)
                  .WithMany()
                  .HasForeignKey(e => e.AddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParentRealty)
                  .WithMany(e => e.ChildRealties)
                  .HasForeignKey(e => e.ParentRealtyId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Listings)
                  .WithOne(e => e.Realty)
                  .HasForeignKey(e => e.RealtyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Photos)
                  .WithOne()
                  .HasForeignKey(e => e.EntityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация Apartment
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.ToTable("Apartments");
            
            entity.Property(e => e.ApartmentNumber).HasMaxLength(20);

            // Связи для справочников
            entity.HasOne(e => e.RenovationType)
                  .WithMany()
                  .HasForeignKey(e => e.RenovationTypeId)
                  .IsRequired(false);

            entity.HasOne(e => e.BathroomType)
                  .WithMany()
                  .HasForeignKey(e => e.BathroomTypeId)
                  .IsRequired(false);
        });

        // Конфигурация PrivateHouse
        modelBuilder.Entity<PrivateHouse>(entity =>
        {
            entity.ToTable("PrivateHouses");

            // Связи для справочников
            entity.HasOne(e => e.HeatingType)
                  .WithMany()
                  .HasForeignKey(e => e.HeatingTypeId)
                  .IsRequired(false);

            entity.HasOne(e => e.ConstructionMaterial)
                  .WithMany()
                  .HasForeignKey(e => e.ConstructionMaterialId)
                  .IsRequired(false);
        });

        // Конфигурация Area
        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Areas");
            entity.Property(e => e.Square).IsRequired();

            entity.HasOne(e => e.LandCategory)
                  .WithMany()
                  .HasForeignKey(e => e.LandCategoryId)
                  .IsRequired(false);
        });

        // Конфигурация Listing
        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.Terms).HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Связи
            entity.HasOne(e => e.Realty)
                  .WithMany(e => e.Listings)
                  .HasForeignKey(e => e.RealtyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Owner)
                  .WithMany()
                  .HasForeignKey(e => e.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ResponsibleEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.ResponsibleEmployeeId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Currency)
                  .WithMany()
                  .HasForeignKey(e => e.CurrencyId)
                  .IsRequired(false);

            entity.HasOne(e => e.ListingType)
                  .WithMany()
                  .HasForeignKey(e => e.ListingTypeId)
                  .IsRequired();

            entity.HasOne(e => e.Status)
                  .WithMany()
                  .HasForeignKey(e => e.StatusId)
                  .IsRequired(false);
        });

        // Конфигурация ClientRequest
        modelBuilder.Entity<ClientRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.CompletedDate).IsRequired(false);
            entity.Property(e => e.ClientId).IsRequired();
            entity.Property(e => e.EmployeeId).IsRequired();
            entity.Property(e => e.DesiredLocation).HasMaxLength(200);
            entity.Property(e => e.AdditionalRequirements).HasMaxLength(1000);
            entity.Property(e => e.ListingId).IsRequired(false);
            entity.Property(e => e.MatchedRequestId).IsRequired(false);
            entity.Property(e => e.DealId).IsRequired(false);
            entity.Property(e => e.DesiredRealtyType).IsRequired(false);

            // Связи
            entity.HasOne(e => e.Client)
                  .WithMany()
                  .HasForeignKey(e => e.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Listing)
                  .WithMany(e => e.ClientRequests)
                  .HasForeignKey(e => e.ListingId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MatchedRequest)
                  .WithMany()
                  .HasForeignKey(e => e.MatchedRequestId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Deal)
                  .WithMany()
                  .HasForeignKey(e => e.DealId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация Deal
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.ListingId).IsRequired();
            entity.Property(e => e.BuyerId).IsRequired();
            entity.Property(e => e.EmployeeId).IsRequired(false);
            entity.Property(e => e.FinalPrice).IsRequired();
            entity.Property(e => e.Commission).IsRequired();
            entity.Property(e => e.DealDate).IsRequired();
            entity.Property(e => e.DealTypeId).IsRequired(false);
            entity.Property(e => e.StatusId).IsRequired();

            // Связи
            entity.HasOne(e => e.Listing)
                  .WithMany()
                  .HasForeignKey(e => e.ListingId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Buyer)
                  .WithMany()
                  .HasForeignKey(e => e.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DealType)
                  .WithMany()
                  .HasForeignKey(e => e.DealTypeId)
                  .IsRequired(false);

            entity.HasOne(e => e.Status)
                  .WithMany()
                  .HasForeignKey(e => e.StatusId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Participants)
                  .WithOne(e => e.Deal)
                  .HasForeignKey(e => e.DealId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация DealParticipant
        modelBuilder.Entity<DealParticipant>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.DealId).IsRequired();
            entity.Property(e => e.ClientRequestId).IsRequired();
            entity.Property(e => e.Role).IsRequired();

            // Связи
            entity.HasOne(e => e.Deal)
                  .WithMany(e => e.Participants)
                  .HasForeignKey(e => e.DealId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ClientRequest)
                  .WithMany()
                  .HasForeignKey(e => e.ClientRequestId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация Dictionary
        modelBuilder.Entity<Dictionary>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasMaxLength(100).IsRequired();
        });

        // Конфигурация DictionaryValue
        modelBuilder.Entity<DictionaryValue>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.DictionaryId).IsRequired();
            entity.Property(e => e.Value).HasMaxLength(200).IsRequired();

            // Связи
            entity.HasOne(e => e.Dictionary)
                  .WithMany()
                  .HasForeignKey(e => e.DictionaryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация Photo
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.EntityType).IsRequired();
            entity.Property(e => e.EntityId).IsRequired();
            entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.ContentType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.SortOrder).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.FileData).IsRequired();
        });

        // Конфигурация View PotentialMatch
        modelBuilder.Entity<PotentialMatch>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("PotentialMatches"); // Укажите имя view в БД

            entity.Property(e => e.BuyRequestId).IsRequired();
            entity.Property(e => e.SellRequestId).IsRequired();
            entity.Property(e => e.BuyerId).IsRequired();
            entity.Property(e => e.SellerId).IsRequired();
            entity.Property(e => e.ListingPrice).IsRequired();
            entity.Property(e => e.RealtyId).IsRequired();
            entity.Property(e => e.SellListingId).IsRequired();
            entity.Property(e => e.RealtyType).IsRequired();
            entity.Property(e => e.RealtyName).IsRequired();
            entity.Property(e => e.City).IsRequired();
            entity.Property(e => e.District).IsRequired();
            entity.Property(e => e.DesiredLocation).IsRequired();
            entity.Property(e => e.BuyType).IsRequired();
            entity.Property(e => e.SellType).IsRequired();
        });

        // Индексы для улучшения производительности
        modelBuilder.Entity<Client>().HasIndex(e => e.Phone);
        modelBuilder.Entity<Employee>().HasIndex(e => e.Login);
        modelBuilder.Entity<Realty>().HasIndex(e => e.AddressId);
        modelBuilder.Entity<Realty>().HasIndex(e => e.ParentRealtyId);
        modelBuilder.Entity<Realty>().HasIndex(e => e.RealtyType);
        modelBuilder.Entity<Listing>().HasIndex(e => e.RealtyId);
        modelBuilder.Entity<Listing>().HasIndex(e => e.OwnerId);
        modelBuilder.Entity<Listing>().HasIndex(e => e.ResponsibleEmployeeId);
        modelBuilder.Entity<Listing>().HasIndex(e => e.CreatedDate);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.ClientId);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.EmployeeId);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.ListingId);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.MatchedRequestId);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.Type);
        modelBuilder.Entity<ClientRequest>().HasIndex(e => e.Status);
        modelBuilder.Entity<Deal>().HasIndex(e => e.ListingId);
        modelBuilder.Entity<Deal>().HasIndex(e => e.BuyerId);
        modelBuilder.Entity<Deal>().HasIndex(e => e.EmployeeId);
        modelBuilder.Entity<Deal>().HasIndex(e => e.DealDate);
        modelBuilder.Entity<DictionaryValue>().HasIndex(e => e.DictionaryId);
        modelBuilder.Entity<Photo>().HasIndex(e => new { e.EntityType, e.EntityId });
    }
    
    /// <summary>
    /// Переопределение SaveChangesAsync для автоматической установки дат
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseIdEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                // Для новых фото устанавливаем дату создания
                if (entityEntry.Entity is Photo photo && photo.CreatedDate == default)
                {
                    photo.CreatedDate = DateTime.UtcNow;
                }
                
                // Для новых сделок устанавливаем дату сделки
                if (entityEntry.Entity is Deal deal && deal.DealDate == default)
                {
                    deal.DealDate = DateTime.UtcNow;
                }
                
                // Для новых запросов устанавливаем дату создания
                if (entityEntry.Entity is ClientRequest request && request.CreatedDate == default)
                {
                    request.CreatedDate = DateTime.UtcNow;
                }
                
                // Для новых объявлений устанавливаем дату создания
                if (entityEntry.Entity is Listing listing && listing.CreatedDate == default)
                {
                    listing.CreatedDate = DateTime.UtcNow;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}