using AutoMapper;
using Data.Layer.Entities;
using Services.Layer.Dtos;

namespace Services.Layer.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>()
                .ForMember(t => t.UserName, opt => opt.MapFrom(t => t.User.UserName))
                .ReverseMap(); // AutoMapper will map Ticket to TicketDto and vice versa
        }
    }
}
