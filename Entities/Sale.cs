namespace Practice_EF.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        public int EstateObjectId { get; set; }
        public EstateObject? EstateObject { get; set; }

        public int AgentId { get; set; }
        public Agent? Agent { get; set; }
    }
}