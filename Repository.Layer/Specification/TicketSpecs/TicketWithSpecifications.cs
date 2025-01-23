using Data.Layer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specification.TicketSpecs
{
    public class TicketWithSpecifications : BaseSpecifications<Ticket>
    {
        public TicketWithSpecifications(TicketSpecification specs)
            : base(ticket =>
                (string.IsNullOrEmpty(specs.UserId) || ticket.UserId == specs.UserId) && // Filter by UserId (if provided)
                (string.IsNullOrEmpty(specs.Title) || ticket.Title.Equals(specs.Title, StringComparison.OrdinalIgnoreCase)) // Filter by Title (if provided)
            )
        {
            // Include the User navigation property
            AddInclude(ticket => ticket.User);

            // Apply sorting
            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort.ToLower()) // Case-insensitive sorting
                {
                    case "titleAsc":
                        AddOrderByAsc(ticket => ticket.Title);
                        break;
                    case "titleDesc":
                        AddOrderByDesc(ticket => ticket.Title);
                        break;
                    case "statusAsc":
                        AddOrderByAsc(ticket => ticket.Status);
                        break;
                    case "statusDesc":
                        AddOrderByDesc(ticket => ticket.Status);
                        break;
                    // Add more sorting options as needed
                    default:
                        AddOrderByAsc(ticket => ticket.Title); // Default sorting
                        break;
                }
            }
        }

        public TicketWithSpecifications(Guid? Id) : base(ticket =>
        (Id != null && // Ensure specs is not null
        (ticket.Id == Id))
        )
        {
            AddInclude(ticket => ticket.User);
        }
    }
}
