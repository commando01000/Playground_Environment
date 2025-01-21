using AutoMapper;
using Data.Layer.Entities;
using Microsoft.Extensions.Configuration;
using Repository.Layer.Interfaces;
using Repository.Layer.Specification.TicketSpecs;
using Services.Layer.Dtos;

namespace Services.Layer.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TicketDto>> GetTicketsAsync(TicketSpecification specs)
        {
            var TicketSpec = new TicketWithSpecifications(specs);
            var Tickets = await _unitOfWork.Repository<Ticket, Guid>().GetAllWithSpecs(TicketSpec); // with include
            // map tickets to tickets dto
            var TicketsDto = _mapper.Map<IEnumerable<TicketDto>>(Tickets);
            return TicketsDto;
        }

        public Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TicketDto> GetTicketAsync(string Id)
        {
            var TicketSpec = new TicketWithSpecifications(Guid.Parse(Id));
            var Tickets = await _unitOfWork.Repository<Ticket, Guid>().GetByIdWithSpecs(TicketSpec); // with include
            // map tickets to tickets dto
            var TicketDto = _mapper.Map<TicketDto>(Tickets);
            return TicketDto;
        }

        public Task<TicketDto> UpdateTicketAsync(TicketDto ticketDto)
        {
            throw new NotImplementedException();
        }
    }
}
