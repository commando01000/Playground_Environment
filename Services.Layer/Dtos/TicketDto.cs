using Data.Layer.Entities;

namespace Services.Layer.Dtos
{
    public class TicketDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public string Priority { get; set; } // Low, Medium, High
        public string UserName { get; set; }
    }
}
