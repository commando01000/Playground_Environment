using Repository.Layer.Specification.TicketSpecs;
using Services.Layer.Dtos;

namespace Services.Layer.Services
{
    public interface ITicketService
    {
        public Task<IEnumerable<TicketDto>> GetTicketsAsync(TicketSpecification spec);
        public Task<TicketDto> GetTicketAsync(string Id);
        public Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        public Task<TicketDto> UpdateTicketAsync(TicketDto ticketDto);
        public Task DeleteTicketAsync(Guid id);
    }
}
