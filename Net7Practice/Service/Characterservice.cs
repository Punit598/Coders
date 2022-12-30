using AutoMapper;
using Net7Practice.Dtos.Character;
using Net7Practice.Models;

namespace Net7Practice.Service
{
    public class Characterservice : ICharacterService
    {
        private static List<Characteristic> Knight = new List<Characteristic> { new Characteristic { Name = "Jacky" }, new Characteristic { } };

        private static IMapper _mapper;
        private static DataContext _context;
        public Characterservice(IMapper mapper, DataContext dataContext) 
        {
            _context= dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterdto>>> GetAllCharacters() 
        {
            var response = new ServiceResponse<List<GetCharacterdto>>();
            var list = await _context.characteristics.ToListAsync();
            response.Data =  list.Select( x => _mapper.Map<GetCharacterdto>(x)).ToList();
            return response;
        }
        public async Task<ServiceResponse<GetCharacterdto>> GetCharactersbyId(int Id) 
        {
            var response = new ServiceResponse<GetCharacterdto>();
            var list = await _context.characteristics.FirstOrDefaultAsync(x => x.Id == Id);
            response.Data = _mapper.Map<GetCharacterdto>(list);
            return response;
        }
        public async Task<ServiceResponse<List<GetCharacterdto>>> AddCharacter(Addcharacterdto addcharacteristic) 
        {
            var response = new ServiceResponse<List<GetCharacterdto>>();

            try
            { 
            var character = _mapper.Map<Characteristic>(addcharacteristic);
            _context.characteristics.Add(character);
                await _context.SaveChangesAsync();
            response.Data = await _context.characteristics.Select(x => _mapper.Map<GetCharacterdto>(x)).ToListAsync();
            return response;
            }
            catch (Exception ex)
            {
                response.Issuccess = false;
                response.Message = ex.Message;
                return response;  
            }
        }
    }
}
