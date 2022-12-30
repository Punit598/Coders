using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Net7Practice.Dtos.Character;
using Net7Practice.Migrations;
using Net7Practice.Models;
using System.Security.Claims;

namespace Net7Practice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {

        private static ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        //[Route("Get")]
        public async Task<ActionResult<ServiceResponse<GetCharacterdto>>> Get()
        {
            var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value; 
            
            var list = await _characterService.GetAllCharacters();
            return Ok(list);

        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterdto>>> Get(int Id)
        {
            var list = await _characterService.GetCharactersbyId(Id);
            if (list is not null)
                return Ok(list);

            return BadRequest("Result Not found");
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterdto>>>>  Post(Addcharacterdto characteristic)
        {
            var Post = await _characterService.AddCharacter(characteristic);
            return Ok(Post);
        }

    }
}
