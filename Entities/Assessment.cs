namespace Practice_EF.Entities
{
    public class Assessment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AssessmentValue { get; set; }

        public int EstateObjectId { get; set; }
        public EstateObject? EstateObject { get; set; }

        public int CriteriaId { get; set; }
        public Criteria? Criteria { get; set; }
    }
}