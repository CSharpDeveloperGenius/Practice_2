namespace Practice_EF.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}