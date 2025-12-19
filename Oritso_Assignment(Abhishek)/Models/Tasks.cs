namespace Oritso_Assignment_Abhishek_.Models
{
    public class Tasks
    {
        public string? Id { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public string Status { get; set; }

        public string? Remarks { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public string? CreatedById { get; set; }

        public string? CreatedByName { get; set; }

        public string? LastUpdatedById { get; set; }

        public string? LastUpdatedByName { get; set; }
    }
}
