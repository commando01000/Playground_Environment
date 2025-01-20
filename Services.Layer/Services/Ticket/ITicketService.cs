using Services.Layer.Dtos;

namespace Services.Layer.Services
{
    public interface ITicketService
    {
        public Task<IEnumerable<TicketDto>> GetTicketsAsync();
        public Task<TicketDto> GetTicketAsync(Guid id);
        public Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        public Task<TicketDto> UpdateTicketAsync(TicketDto ticketDto);
        public Task DeleteTicketAsync(Guid id);
    }
}
