using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.Models.DbEntities;
using Attribute = RealtorTool.Core.Models.DbEntities.Attribute;

namespace RealtorTool.Data.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DataContext()
    { }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=RealtorTools;");

    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Dictionary> Dictionaries { get; set; }
    public DbSet<DictionaryValue> DictionaryValues { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Realty> Realties { get; set; }
    public DbSet<RealtyAttribute> RealtyAttributes { get; set; }
    public DbSet<RealtyRequest> RealtyRequests { get; set; }
    public DbSet<Request> Requests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация Attribute
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
        });

        // Конфигурация Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(c => c.Fname).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Sname).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Lname).HasMaxLength(50);
        });

        // Конфигурация Dictionary
        modelBuilder.Entity<Dictionary>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(d => d.Type).IsRequired().HasMaxLength(50);
            entity.HasIndex(d => d.Type).IsUnique();
        });

        // Конфигурация DictionaryValue
        modelBuilder.Entity<DictionaryValue>(entity =>
        {
            entity.HasKey(dv => dv.Id);
            entity.Property(dv => dv.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(dv => dv.Dictionaryid).IsRequired();
            entity.Property(dv => dv.Value).IsRequired().HasMaxLength(100);

            entity.HasOne(dv => dv.Dictionary)
                .WithMany(d => d.DictionaryValues)
                .HasForeignKey(dv => dv.Dictionaryid)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Fname).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Sname).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Lname).HasMaxLength(50);
            entity.Property(e => e.Login).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Login).IsUnique();
        });

        // Конфигурация Realty
        modelBuilder.Entity<Realty>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(r => r.TypeId).IsRequired();
            entity.Property(r => r.Name).IsRequired().HasMaxLength(200);
            entity.Property(r => r.Address).IsRequired().HasMaxLength(500);
            entity.Property(r => r.Price).HasPrecision(18, 2);
            entity.Property(r => r.Currency).HasMaxLength(3).HasDefaultValue("RUB");
            entity.Property(r => r.StatusId).IsRequired();
            entity.Property(r => r.CreatedDate).HasDefaultValueSql("NOW()");
            entity.Property(r => r.Attributes).HasColumnType("jsonb");

            entity.HasOne(r => r.Type)
                .WithMany(dv => dv.Realties)
                .HasForeignKey(r => r.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Status)
                .WithMany()
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Конфигурация RealtyAttribute
        modelBuilder.Entity<RealtyAttribute>(entity =>
        {
            entity.HasKey(ra => new { ra.IdRealty, ra.IdAttribute });

            entity.HasOne(ra => ra.Realty)
                .WithMany(r => r.RealtyAttributes)
                .HasForeignKey(ra => ra.IdRealty)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ra => ra.Attribute)
                .WithMany(a => a.RealtyAttributes)
                .HasForeignKey(ra => ra.IdAttribute)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация RealtyRequest
        modelBuilder.Entity<RealtyRequest>(entity =>
        {
            entity.HasKey(rr => rr.Id);
            entity.Property(rr => rr.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(rr => rr.Realty)
                .WithMany(r => r.RequestsLink)
                .HasForeignKey(rr => rr.RealtyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rr => rr.Request)
                .WithMany(r => r.RealtyLink)
                .HasForeignKey(rr => rr.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация Request
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(r => r.EmployeeId).IsRequired();
            entity.Property(r => r.TypeId).IsRequired();
            entity.Property(r => r.StatusId).IsRequired();
            entity.Property(r => r.Priority).HasDefaultValue(1);
            entity.Property(r => r.CreatedDate).HasDefaultValueSql("NOW()");
            entity.Property(r => r.RequestData).HasColumnType("jsonb");

            entity.HasOne(r => r.Client)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(r => r.Employee)
                .WithMany(e => e.Requests)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Type)
                .WithMany()
                .HasForeignKey(r => r.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Status)
                .WithMany()
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}