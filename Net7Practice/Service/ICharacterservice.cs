using Net7Practice.Dtos.Character;

namespace Net7Practice.Service
{
    public interface ICharacterService
    {

       Task<ServiceResponse<List<GetCharacterdto>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterdto>> GetCharactersbyId(int Id);
        Task<ServiceResponse<List<GetCharacterdto>>> AddCharacter(Addcharacterdto characteristic);
    }
}
