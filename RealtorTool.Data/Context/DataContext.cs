using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
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
    
    /// <summary>
    /// Конфигурация подключения к PostgreSQL
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=RT;");

    // Основные DbSet
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Dictionary> Dictionaries { get; set; } = null!;
    public DbSet<DictionaryValue> DictionaryValues { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; } = null!;
    public DbSet<ClientRequest> ClientRequests { get; set; } = null!;
    
    // Недвижимость и её наследники (TPT стратегия)
    public DbSet<Realty> Realties { get; set; } = null!;
    public DbSet<Apartment> Apartments { get; set; } = null!;
    public DbSet<PrivateHouse> PrivateHouses { get; set; } = null!;
    public DbSet<Area> Areas { get; set; } = null!;
    
    // Предложения и сделки
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Deal> Deals { get; set; } = null!;
    public DbSet<DealParticipant> DealParticipants { get; set; } = null!;
    public DbSet<PotentialMatch> PotentialMatches { get; set; } = null!;

    /// <summary>
    /// Конфигурация модели данных
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table Per Type (TPT) для наследования Realty
        modelBuilder.Entity<Realty>().ToTable("Realties");
        modelBuilder.Entity<Apartment>().ToTable("Apartments");
        modelBuilder.Entity<PrivateHouse>().ToTable("PrivateHouses");
        modelBuilder.Entity<Area>().ToTable("Areas");

        // Table Per Type (TPT) для наследования Photo
        modelBuilder.Entity<Photo>().ToTable("Photos");
        modelBuilder.Entity<RealtyPhoto>().ToTable("RealtyPhotos");
        modelBuilder.Entity<ClientPhoto>().ToTable("ClientPhotos");
        modelBuilder.Entity<EmployeePhoto>().ToTable("EmployeePhotos");

        // Конфигурация Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasIndex(a => a.City);
            entity.HasIndex(a => a.District);
            entity.HasIndex(a => a.PostalCode);
            
            // Уникальный индекс для предотвращения дубликатов
            entity.HasIndex(a => new { a.City, a.District, a.Street, a.HouseNumber, a.BuildingNumber })
                .IsUnique();
        });

        // Конфигурация Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Phone).IsUnique();
            
            // Связь с ClientPhoto
            entity.HasOne(c => c.Photo)
                .WithOne(cp => cp.Client)
                .HasForeignKey<ClientPhoto>(cp => cp.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Конфигурация Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Login).IsUnique();
            
            // Связь с EmployeePhoto
            entity.HasOne(e => e.Photo)
                .WithOne(ep => ep.Employee)
                .HasForeignKey<EmployeePhoto>(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Конфигурация Realty
        modelBuilder.Entity<Realty>(entity =>
        {
            entity.HasKey(r => r.Id);

            // Связь с Address
            entity.HasOne(r => r.Address)
                .WithMany()
                .HasForeignKey(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Самореференциальная связь (Parent-Child)
            entity.HasOne(r => r.ParentRealty)
                .WithMany(r => r.ChildRealties)
                .HasForeignKey(r => r.ParentRealtyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с RealtyPhoto
            entity.HasMany(r => r.Photos)
                .WithOne(rp => rp.Realty)
                .HasForeignKey(rp => rp.RealtyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы
            entity.HasIndex(r => r.IsActive);
            entity.HasIndex(r => r.AddressId);
            entity.HasIndex(r => r.ParentRealtyId);
            entity.HasIndex(r => r.RealtyType);
        });

        // Конфигурация Apartment
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasOne(a => a.RenovationType)
                .WithMany()
                .HasForeignKey(a => a.RenovationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.BathroomType)
                .WithMany()
                .HasForeignKey(a => a.BathroomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => a.RenovationTypeId);
            entity.HasIndex(a => a.BathroomTypeId);
            entity.HasIndex(a => a.Floor);
            entity.HasIndex(a => a.RoomsCount);
            entity.HasIndex(a => a.TotalArea);
        });

        // Конфигурация PrivateHouse
        modelBuilder.Entity<PrivateHouse>(entity =>
        {
            entity.HasOne(ph => ph.HeatingType)
                .WithMany()
                .HasForeignKey(ph => ph.HeatingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ph => ph.ConstructionMaterial)
                .WithMany()
                .HasForeignKey(ph => ph.ConstructionMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(ph => ph.HeatingTypeId);
            entity.HasIndex(ph => ph.ConstructionMaterialId);
            entity.HasIndex(ph => ph.YearBuilt);
            entity.HasIndex(ph => ph.FloorsCount);
            entity.HasIndex(ph => ph.RoomsCount);
        });

        // Конфигурация Area
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasOne(a => a.LandCategory)
                .WithMany()
                .HasForeignKey(a => a.LandCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => a.LandCategoryId);
            entity.HasIndex(a => a.Square);
        });

        // Конфигурация ClientRequest (УБРАНА связь с Realty)
        modelBuilder.Entity<ClientRequest>(entity =>
        {
            entity.HasKey(cr => cr.Id);

            // Связь с Client
            entity.HasOne(cr => cr.Client)
                .WithMany()
                .HasForeignKey(cr => cr.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Employee
            entity.HasOne(cr => cr.Employee)
                .WithMany()
                .HasForeignKey(cr => cr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Listing
            entity.HasOne(cr => cr.Listing)
                .WithMany(l => l.ClientRequests)
                .HasForeignKey(cr => cr.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            // УБРАНА связь с Realty
            // entity.HasOne(cr => cr.Realty)...

            // Самореференциальная связь (MatchedRequest)
            entity.HasOne(cr => cr.MatchedRequest)
                .WithMany()
                .HasForeignKey(cr => cr.MatchedRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Deal через DealParticipant (обратная связь)
            entity.HasMany<DealParticipant>()
                .WithOne(dp => dp.ClientRequest)
                .HasForeignKey(dp => dp.ClientRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Индексы
            entity.HasIndex(cr => cr.Type);
            entity.HasIndex(cr => cr.Status);
            entity.HasIndex(cr => cr.CreatedDate);
            entity.HasIndex(cr => cr.ClientId);
            entity.HasIndex(cr => cr.EmployeeId);
            entity.HasIndex(cr => cr.ListingId);
            entity.HasIndex(cr => cr.MatchedRequestId);
            entity.HasIndex(cr => new { cr.Status, cr.Type });
        });

        // Конфигурация Listing
        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(l => l.Id);

            // Связь с Realty
            entity.HasOne(l => l.Realty)
                .WithMany(r => r.Listings)
                .HasForeignKey(l => l.RealtyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Client (Owner)
            entity.HasOne(l => l.Owner)
                .WithMany()
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Employee (ResponsibleEmployee)
            entity.HasOne(l => l.ResponsibleEmployee)
                .WithMany()
                .HasForeignKey(l => l.ResponsibleEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DictionaryValue (Currency)
            entity.HasOne(l => l.Currency)
                .WithMany()
                .HasForeignKey(l => l.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DictionaryValue (ListingType)
            entity.HasOne(l => l.ListingType)
                .WithMany()
                .HasForeignKey(l => l.ListingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DictionaryValue (Status)
            entity.HasOne(l => l.Status)
                .WithMany()
                .HasForeignKey(l => l.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Индексы
            entity.HasIndex(l => l.RealtyId);
            entity.HasIndex(l => l.OwnerId);
            entity.HasIndex(l => l.ResponsibleEmployeeId);
            entity.HasIndex(l => l.CurrencyId);
            entity.HasIndex(l => l.ListingTypeId);
            entity.HasIndex(l => l.StatusId);
            entity.HasIndex(l => l.Price);
            entity.HasIndex(l => l.CreatedDate);
            entity.HasIndex(l => new { l.StatusId, l.CreatedDate });
        });

        // Конфигурация Deal (ОБНОВЛЕНА - убрана связь с ClientRequest)
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(d => d.Id);

            // Связь с Listing
            entity.HasOne(d => d.Listing)
                .WithMany()
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Client (Buyer) - оставляем для быстрого доступа к основному покупателю
            entity.HasOne(d => d.Buyer)
                .WithMany()
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с Employee
            entity.HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DictionaryValue (DealType)
            entity.HasOne(d => d.DealType)
                .WithMany()
                .HasForeignKey(d => d.DealTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DictionaryValue (Status)
            entity.HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с DealParticipant
            entity.HasMany(d => d.Participants)
                .WithOne(dp => dp.Deal)
                .HasForeignKey(dp => dp.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы
            entity.HasIndex(d => d.ListingId);
            entity.HasIndex(d => d.BuyerId);
            entity.HasIndex(d => d.EmployeeId);
            entity.HasIndex(d => d.DealTypeId);
            entity.HasIndex(d => d.StatusId);
            entity.HasIndex(d => d.DealDate);
            entity.HasIndex(d => d.FinalPrice);
            entity.HasIndex(d => new { d.StatusId, d.DealDate });
        });

        // Конфигурация DealParticipant (НОВАЯ сущность)
        modelBuilder.Entity<DealParticipant>(entity =>
        {
            entity.HasKey(dp => dp.Id);

            // Связь с Deal
            entity.HasOne(dp => dp.Deal)
                .WithMany(d => d.Participants)
                .HasForeignKey(dp => dp.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с ClientRequest
            entity.HasOne(dp => dp.ClientRequest)
                .WithMany()
                .HasForeignKey(dp => dp.ClientRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Уникальный индекс - одна заявка не может участвовать дважды в одной сделке
            entity.HasIndex(dp => new { dp.DealId, dp.ClientRequestId })
                .IsUnique();

            // Индекс для поиска по роли
            entity.HasIndex(dp => dp.Role);
            
            // Индекс для поиска всех сделок заявки
            entity.HasIndex(dp => dp.ClientRequestId);
        });

        // Конфигурация DictionaryValue
        modelBuilder.Entity<DictionaryValue>(entity =>
        {
            entity.HasKey(dv => dv.Id);

            entity.HasOne(dv => dv.Dictionary)
                .WithMany()
                .HasForeignKey(dv => dv.DictionaryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(dv => dv.DictionaryId);
            entity.HasIndex(dv => dv.Value);
            entity.HasIndex(dv => new { dv.DictionaryId, dv.Value }).IsUnique();
        });

        // Конфигурация Dictionary
        modelBuilder.Entity<Dictionary>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasIndex(d => d.Type).IsUnique();
        });

        // Конфигурация Photo (базовый класс)
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            // Общие индексы для всех фото
            entity.HasIndex(p => p.IsMain);
            entity.HasIndex(p => p.SortOrder);
            entity.HasIndex(p => p.CreatedDate);

            // Ограничение на размер файла (20MB максимум)
            entity.Property(p => p.FileData)
                .HasMaxLength(20 * 1024 * 1024);
        });

        // Конфигурация RealtyPhoto
        modelBuilder.Entity<RealtyPhoto>(entity =>
        {
            // Связь с Realty
            entity.HasOne(rp => rp.Realty)
                .WithMany(r => r.Photos)
                .HasForeignKey(rp => rp.RealtyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(rp => rp.RealtyId);
        });

        // Конфигурация ClientPhoto
        modelBuilder.Entity<ClientPhoto>(entity =>
        {
            // Связь с Client
            entity.HasOne(cp => cp.Client)
                .WithOne(c => c.Photo)
                .HasForeignKey<ClientPhoto>(cp => cp.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(cp => cp.ClientId).IsUnique();
        });

        // Конфигурация EmployeePhoto
        modelBuilder.Entity<EmployeePhoto>(entity =>
        {
            // Связь с Employee
            entity.HasOne(ep => ep.Employee)
                .WithOne(e => e.Photo)
                .HasForeignKey<EmployeePhoto>(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(ep => ep.EmployeeId).IsUnique();
        });
        
        modelBuilder.Entity<PotentialMatch>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("PotentialMatches");
        
            // Маппинг свойств
            // entity.Property(e => e.BuyRequestId).HasColumnName("buy_request_id");
            // entity.Property(e => e.SellRequestId).HasColumnName("sell_request_id");
            // entity.Property(e => e.BuyType).HasColumnName("buy_type");
            // entity.Property(e => e.SellType).HasColumnName("sell_type");
            // entity.Property(e => e.MaxPrice).HasColumnName("max_price");
            // entity.Property(e => e.ListingPrice).HasColumnName("listing_price");
            // entity.Property(e => e.RealtyType).HasColumnName("realty_type");
            // entity.Property(e => e.TotalArea).HasColumnName("total_area");
            // entity.Property(e => e.RoomsCount).HasColumnName("rooms_count");
            // entity.Property(e => e.City).HasColumnName("city");
            // entity.Property(e => e.District).HasColumnName("district");
            // entity.Property(e => e.MinRooms).HasColumnName("min_rooms");
            // entity.Property(e => e.MinArea).HasColumnName("min_area");
            // entity.Property(e => e.MaxArea).HasColumnName("max_area");
            // entity.Property(e => e.DesiredLocation).HasColumnName("desired_location");
        
            // Игнорируем вычисляемые свойства для EF
            entity.Ignore(e => e.MatchScore);
            entity.Ignore(e => e.IsGoodMatch);
            entity.Ignore(e => e.IsPerfectMatch);
            entity.Ignore(e => e.MatchDescription);
            entity.Ignore(e => e.PriceDifference);
            entity.Ignore(e => e.IsWithinBudget);
        });
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