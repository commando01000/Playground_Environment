using Data.Layer.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Layer.Entities
{
    public enum TicketStatus
    {
        Open,         // Ticket is newly created
        InProgress,   // Ticket is being worked on
        Resolved      // Ticket has been resolved
    }
    public class Ticket : BaseEntity<Guid>
    {
        [Required(ErrorMessage = "Title is required"), MaxLength(100), MinLength(5), Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required"), MaxLength(1000), MinLength(10), Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Status is required"), Display(Name = "Status")]
        public TicketStatus Status { get; set; } = TicketStatus.Open; // Default status is "Open"
        [Required(ErrorMessage = "Priority is required"), Display(Name = "Priority")]
        public string Priority { get; set; } // Low, Medium, High
        public string UserId { get; set; } // Foreign key to the user who created the ticket
        public AppUser User { get; set; }
    }
}
