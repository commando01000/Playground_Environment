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
            .ForMember(t => t.UserName, opt => opt.MapFrom(t => t.User != null ? t.User.UserName : "Unknown Username"))
            .ReverseMap();
        }
    }
}
