using Microsoft.EntityFrameworkCore;
using RealtorTool.Models.DbEntities;
using Attribute = RealtorTool.Models.DbEntities.Attribute;

namespace RealtorTool.Context;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Dictionary> Dictionaries { get; set; }

    public virtual DbSet<DictionaryValue> DictionaryValues { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Realty> Realties { get; set; }

    public virtual DbSet<RealtyAttribute> RealtyAttributes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5415;Database=RealtorTool;Username=postgres;Password=toor;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attributes_pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Fname)
                .HasColumnType("character varying")
                .HasColumnName("fname");
            entity.Property(e => e.Lname)
                .HasColumnType("character varying")
                .HasColumnName("lname");
            entity.Property(e => e.Sname)
                .HasColumnType("character varying")
                .HasColumnName("sname");
        });

        modelBuilder.Entity<Dictionary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dictionaries_pk");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<DictionaryValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dictionaryvalues_pk");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Dictionaryid).HasColumnName("dictionaryid");
            entity.Property(e => e.Value)
                .HasColumnType("character varying")
                .HasColumnName("value");

            entity.HasOne(d => d.Dictionary).WithMany(p => p.DictionaryValues)
                .HasForeignKey(d => d.Dictionaryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dictionaryvalues_dictionaries_id_fk");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employees_pk");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Fname)
                .HasColumnType("character varying")
                .HasColumnName("fname");
            entity.Property(e => e.Lname)
                .HasColumnType("character varying")
                .HasColumnName("lname");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Sname)
                .HasColumnType("character varying")
                .HasColumnName("sname");
        });

        modelBuilder.Entity<Realty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("realties_pk");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Realty1)
                .HasColumnType("jsonb")
                .HasColumnName("realty");
            entity.Property(e => e.Typeid).HasColumnName("typeid");

            entity.HasOne(d => d.Type).WithMany(p => p.Realties)
                .HasForeignKey(d => d.Typeid)
                .HasConstraintName("realties_dictionaryvalues_id_fk");
        });

        modelBuilder.Entity<RealtyAttribute>(entity =>
        {
            entity.HasKey(e => new { Idrealty = e.IdRealty, Idattribute = e.IdAttribute }).HasName("realtyattributes_pk");

            entity.Property(e => e.IdRealty).HasColumnName("idrealty");
            entity.Property(e => e.IdAttribute).HasColumnName("idattribute");
            entity.Property(e => e.Value)
                .HasColumnType("character varying")
                .HasColumnName("value");

            entity.HasOne(d => d.Attribute).WithMany(p => p.RealtyAttributes)
                .HasForeignKey(d => d.IdAttribute)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("realtyattributes_attributes_id_fk");

            entity.HasOne(d => d.Realty).WithMany(p => p.RealtyAttributes)
                .HasForeignKey(d => d.IdRealty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("realtyattributes_realties_id_fk");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requests_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RequestData)
                .HasColumnType("jsonb")
                .HasColumnName("Request");

            entity.HasOne(d => d.Client).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("requests_clients_id_fk");

            entity.HasOne(d => d.Employee).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requests_employees_id_fk");

            entity.HasOne(d => d.Realty).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdRealtyInDeal)
                .HasConstraintName("requests_realties_id_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Requests)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requests_dictionaryvalues_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
