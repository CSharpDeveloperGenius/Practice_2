using Microsoft.EntityFrameworkCore;
using Practice_EF.Entities;

namespace Practice_EF
{
    public class ApplicationDbContext : DbContext
    {
        public string ConnectionString { get; set; }

        public DbSet<Agent> Agents { get; set; } = null!;
        public DbSet<Assessment> Assessments { get; set; } = null!;
        public DbSet<BuildingMaterial> BuildingMaterials { get; set; } = null!;
        public DbSet<Criteria> Criterias { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<EstateObject> EstateObjects { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<ObjectType> ObjectTypes { get; set; } = null!;

        public ApplicationDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ObjectType>().HasData(
                new ObjectType { Id = 1, Title = "Апартаменты" },
                new ObjectType { Id = 2, Title = "Тип 2" },
                new ObjectType { Id = 3, Title = "Тип 3" }
            );

            modelBuilder.Entity<District>().HasData(
                new District { Id = 1, Title = "Преображенский" },
                new District { Id = 2, Title = "Тропарёво-Никулино" },
                new District { Id = 3, Title = "Бескудниковский" }
            );

            modelBuilder.Entity<BuildingMaterial>().HasData(
                new BuildingMaterial { Id = 1, Title = "Материал 1" },
                new BuildingMaterial { Id = 2, Title = "Материал 2" },
                new BuildingMaterial { Id = 3, Title = "Материал 3" }
            );

            modelBuilder.Entity<EstateObject>().HasData(
                new EstateObject
                {
                    Id = 1,
                    Address = "Улица Пушкина, 10",
                    Floor = 3,
                    RoomCount = 3,
                    Status = 1,
                    Price = 75000,
                    Description = "Описание объекта 1",
                    Square = 80,
                    Date = DateTime.UtcNow,
                    DistrictId = 1,
                    ObjectTypeId = 1,
                    BuildingMaterialId = 1
                },
                new EstateObject
                {
                    Id = 2,
                    Address = "Улица Лермонтова, 5",
                    Floor = 5,
                    RoomCount = 2,
                    Status = 1,
                    Price = 60000,
                    Description = "Описание объекта 2",
                    Square = 70,
                    Date = DateTime.UtcNow,
                    DistrictId = 1,
                    ObjectTypeId = 1,
                    BuildingMaterialId = 2
                },
                new EstateObject
                {
                    Id = 3,
                    Address = "Улица Гоголя, 15",
                    Floor = 2,
                    RoomCount = 4,
                    Status = 1,
                    Price = 85000,
                    Description = "Описание объекта 3",
                    Square = 100,
                    Date = DateTime.UtcNow,
                    DistrictId = 2,
                    ObjectTypeId = 1,
                    BuildingMaterialId = 1
                }
            );

            modelBuilder.Entity<Criteria>().HasData(
                new Criteria { Id = 1, Title = "Безопасность" },
                new Criteria { Id = 2, Title = "Качество" },
                new Criteria { Id = 3, Title = "Удобство" }
            );

            modelBuilder.Entity<Assessment>().HasData(
                new Assessment { Id = 1, Date = DateTime.UtcNow, AssessmentValue = 5, EstateObjectId = 1, CriteriaId = 1 },
                new Assessment { Id = 2, Date = DateTime.UtcNow, AssessmentValue = 4, EstateObjectId = 2, CriteriaId = 1 },
                new Assessment { Id = 3, Date = DateTime.UtcNow, AssessmentValue = 3, EstateObjectId = 3, CriteriaId = 1 }
            );

            modelBuilder.Entity<Agent>().HasData(
                new Agent { Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", PhoneNumber = "99999999999" },
                new Agent { Id = 2, LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", PhoneNumber = "88888888888" }
            );

            modelBuilder.Entity<Sale>().HasData(
                new Sale { Id = 1, Date = DateTime.UtcNow, Price = 80000, EstateObjectId = 1, AgentId = 1 },
                new Sale { Id = 2, Date = DateTime.UtcNow, Price = 70000, EstateObjectId = 2, AgentId = 1 },
                new Sale { Id = 3, Date = DateTime.UtcNow, Price = 60000, EstateObjectId = 3, AgentId = 1 }
            );
        }
    }
}