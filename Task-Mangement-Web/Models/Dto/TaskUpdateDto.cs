namespace Task_Mangement_Web.Models.Dto
{
    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
