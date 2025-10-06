using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.Models.DbModels;
using RealtorTool.Data;

namespace RealtorTool.Data.Context;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<AssignmentHistory> AssignmentHistories { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientRequest> ClientRequests { get; set; }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LandPlot> LandPlots { get; set; }

    public virtual DbSet<PrivateHouse> PrivateHouses { get; set; }

    public virtual DbSet<PropertyListing> PropertyListings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=RT;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("Addresses_pkey");

            entity.Property(e => e.ApartmentNumber).HasMaxLength(20);
            entity.Property(e => e.BuildingNumber).HasMaxLength(20);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Россия'::character varying");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.HouseNumber).HasMaxLength(20);
            entity.Property(e => e.Latitude).HasPrecision(10, 8);
            entity.Property(e => e.Longitude).HasPrecision(11, 8);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(255);
        });

        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.ApartmentId).HasName("Apartments_pkey");

            entity.HasIndex(e => e.BuildingId, "IX_Apartments_BuildingId");

            entity.Property(e => e.ApartmentNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HasBalcony).HasDefaultValue(false);
            entity.Property(e => e.HasLoggia).HasDefaultValue(false);
            entity.Property(e => e.KitchenArea).HasPrecision(10, 2);
            entity.Property(e => e.LivingArea).HasPrecision(10, 2);
            entity.Property(e => e.RenovationType).HasMaxLength(50);
            entity.Property(e => e.TotalArea).HasPrecision(10, 2);

            entity.HasOne(d => d.Building).WithMany(p => p.Apartments)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("Apartments_BuildingId_fkey");
        });

        modelBuilder.Entity<AssignmentHistory>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("AssignmentHistory_pkey");

            entity.ToTable("AssignmentHistory");

            entity.HasIndex(e => e.DealId, "IX_AssignmentHistory_DealId");

            entity.HasIndex(e => e.EmployeeId, "IX_AssignmentHistory_EmployeeId");

            entity.HasIndex(e => e.ListingId, "IX_AssignmentHistory_ListingId");

            entity.HasIndex(e => e.RequestId, "IX_AssignmentHistory_RequestId");

            entity.Property(e => e.AssignedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.AssignmentType).HasMaxLength(20);

            entity.HasOne(d => d.Deal).WithMany(p => p.AssignmentHistories)
                .HasForeignKey(d => d.DealId)
                .HasConstraintName("AssignmentHistory_DealId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.AssignmentHistories)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentHistory_EmployeeId_fkey");

            entity.HasOne(d => d.Listing).WithMany(p => p.AssignmentHistories)
                .HasForeignKey(d => d.ListingId)
                .HasConstraintName("AssignmentHistory_ListingId_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.AssignmentHistories)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("AssignmentHistory_RequestId_fkey");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("Buildings_pkey");

            entity.HasIndex(e => e.AddressId, "IX_Buildings_AddressId");

            entity.Property(e => e.ConstructionMaterial).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HasElevator).HasDefaultValue(false);
            entity.Property(e => e.HasParking).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Address).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Buildings_AddressId_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("Clients_pkey");

            entity.HasIndex(e => e.Email, "Unique_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "Unique_Phone").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PassportNumber).HasMaxLength(10);
            entity.Property(e => e.PassportSeries).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.RegistrationAddress).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RegistrationAddressId)
                .HasConstraintName("Clients_RegistrationAddressId_fkey");
        });

        modelBuilder.Entity<ClientRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("ClientRequests_pkey");

            entity.HasIndex(e => e.ClientId, "IX_ClientRequests_ClientId");

            entity.HasIndex(e => e.EmployeeId, "IX_ClientRequests_EmployeeId");

            entity.HasIndex(e => e.PropertyType, "IX_ClientRequests_PropertyType");

            entity.HasIndex(e => e.Status, "IX_ClientRequests_Status");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.MaxArea).HasPrecision(10, 2);
            entity.Property(e => e.MaxPrice).HasPrecision(15, 2);
            entity.Property(e => e.MinArea).HasPrecision(10, 2);
            entity.Property(e => e.MinPrice).HasPrecision(15, 2);
            entity.Property(e => e.PropertyType).HasMaxLength(20);
            entity.Property(e => e.RequestDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.RequestType).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientRequests)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientRequests_ClientId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.ClientRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("ClientRequests_EmployeeId_fkey");
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.DealId).HasName("Deals_pkey");

            entity.HasIndex(e => e.BuyerId, "IX_Deals_BuyerId");

            entity.HasIndex(e => e.EmployeeId, "IX_Deals_EmployeeId");

            entity.HasIndex(e => e.ListingId, "IX_Deals_ListingId");

            entity.HasIndex(e => e.SellerId, "IX_Deals_SellerId");

            entity.Property(e => e.CommissionAmount).HasPrecision(15, 2);
            entity.Property(e => e.ContractNumber).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DealDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.DealStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'InProgress'::character varying");
            entity.Property(e => e.FinalPrice).HasPrecision(15, 2);

            entity.HasOne(d => d.Buyer).WithMany(p => p.DealBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Deals_BuyerId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Deals)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("Deals_EmployeeId_fkey");

            entity.HasOne(d => d.Listing).WithMany(p => p.Deals)
                .HasForeignKey(d => d.ListingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Deals_ListingId_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Deals)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("Deals_RequestId_fkey");

            entity.HasOne(d => d.Seller).WithMany(p => p.DealSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Deals_SellerId_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("Employees_pkey");

            entity.HasIndex(e => e.Email, "Unique_Employee_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "Unique_Employee_Phone").IsUnique();

            entity.Property(e => e.CommissionRate).HasPrecision(5, 2);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Salary).HasPrecision(12, 2);
            entity.Property(e => e.Salt).HasColumnType("character varying");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<LandPlot>(entity =>
        {
            entity.HasKey(e => e.LandPlotId).HasName("LandPlots_pkey");

            entity.HasIndex(e => e.AddressId, "IX_LandPlots_AddressId");

            entity.HasIndex(e => e.CadastralNumber, "LandPlots_CadastralNumber_key").IsUnique();

            entity.Property(e => e.Area).HasPrecision(10, 2);
            entity.Property(e => e.CadastralNumber).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HasUtilities).HasDefaultValue(false);
            entity.Property(e => e.ZoningType).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.LandPlots)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LandPlots_AddressId_fkey");
        });

        modelBuilder.Entity<PrivateHouse>(entity =>
        {
            entity.HasKey(e => e.PrivateHouseId).HasName("PrivateHouses_pkey");

            entity.HasIndex(e => e.LandPlotId, "IX_PrivateHouses_LandPlotId");

            entity.Property(e => e.ConstructionMaterial).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.HasBasement).HasDefaultValue(false);
            entity.Property(e => e.HasGarage).HasDefaultValue(false);
            entity.Property(e => e.HeatingType).HasMaxLength(50);
            entity.Property(e => e.TotalArea).HasPrecision(10, 2);

            entity.HasOne(d => d.LandPlot).WithMany(p => p.PrivateHouses)
                .HasForeignKey(d => d.LandPlotId)
                .HasConstraintName("PrivateHouses_LandPlotId_fkey");
        });

        modelBuilder.Entity<PropertyListing>(entity =>
        {
            entity.HasKey(e => e.ListingId).HasName("PropertyListings_pkey");

            entity.HasIndex(e => e.ListingId, "IX_PropertyListings_ActiveRents").HasFilter("(((\"Status\")::text = 'Active'::text) AND ((\"TransactionType\")::text = 'Rent'::text))");

            entity.HasIndex(e => e.ListingId, "IX_PropertyListings_ActiveSales").HasFilter("(((\"Status\")::text = 'Active'::text) AND ((\"TransactionType\")::text = 'Sale'::text))");

            entity.HasIndex(e => e.ApartmentId, "IX_PropertyListings_ApartmentId").HasFilter("(\"ApartmentId\" IS NOT NULL)");

            entity.HasIndex(e => e.ClientId, "IX_PropertyListings_ClientId");

            entity.HasIndex(e => e.EmployeeId, "IX_PropertyListings_EmployeeId");

            entity.HasIndex(e => e.LandPlotId, "IX_PropertyListings_LandPlotId").HasFilter("(\"LandPlotId\" IS NOT NULL)");

            entity.HasIndex(e => e.PrivateHouseId, "IX_PropertyListings_PrivateHouseId").HasFilter("(\"PrivateHouseId\" IS NOT NULL)");

            entity.HasIndex(e => e.Status, "IX_PropertyListings_Status");

            entity.HasIndex(e => e.TransactionType, "IX_PropertyListings_TransactionType");

            entity.Property(e => e.Commission).HasPrecision(5, 2);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValueSql("'RUB'::character varying");
            entity.Property(e => e.IsExclusive).HasDefaultValue(false);
            entity.Property(e => e.ListingDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Price).HasPrecision(15, 2);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying");
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.Apartment).WithMany(p => p.PropertyListings)
                .HasForeignKey(d => d.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PropertyListings_ApartmentId_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.PropertyListings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PropertyListings_ClientId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.PropertyListings)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("PropertyListings_EmployeeId_fkey");

            entity.HasOne(d => d.LandPlot).WithMany(p => p.PropertyListings)
                .HasForeignKey(d => d.LandPlotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PropertyListings_LandPlotId_fkey");

            entity.HasOne(d => d.PrivateHouse).WithMany(p => p.PropertyListings)
                .HasForeignKey(d => d.PrivateHouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PropertyListings_PrivateHouseId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
