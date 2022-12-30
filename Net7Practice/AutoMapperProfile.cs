using AutoMapper;
using Net7Practice.Dtos.Character;

namespace Net7Practice
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Characteristic, GetCharacterdto>();
            CreateMap<Addcharacterdto,Characteristic> ();
        }
    }
}
