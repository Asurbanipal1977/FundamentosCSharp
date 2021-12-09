using AutoMapper;
using AutoMapperMVC.Models;

namespace AutoMapperMVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Nos permite mapear aunque el nombre de los campos cambie
            CreateMap<ClienteRequest, Cliente>()
                .ForMember(d => d.Name, o=>o.MapFrom(s=>s.Nombre))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.Apellido));
        }
    }
}
