namespace Practice_EF.Entities
{
    public class EstateObject
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public int Floor { get; set; }
        public int RoomCount { get; set; }
        public int Status { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public double Square { get; set; }
        public DateTime Date { get; set; }

        public int DistrictId { get; set; }
        public District? District { get; set; }

        public long ObjectTypeId { get; set; }
        public ObjectType? ObjectType { get; set; }

        public int BuildingMaterialId { get; set; }
        public BuildingMaterial? BuildingMaterial { get; set; }

        public IEnumerable<Assessment> Assessments { get; set; } = [];
        public IEnumerable<Sale> Sales { get; set; } = [];
    }
}