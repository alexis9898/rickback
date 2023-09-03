using BLL.Interface;
using BLL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rick_morty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        [HttpPost("original-charcter")]
        public async Task<IActionResult> GetOriginalCharacters([FromBody] GetCharacter data)
        {
            try
            {
                var userId = data.UserId;
                var character = data.Character;
                if(userId == null || character==null )
                    return BadRequest();
                if(character.OriginalId==null)
                    return NotFound();

                var originalCharacter = await _characterService.GetCharacterAsync((int)character.OriginalId,userId);
                if (originalCharacter == null)
                    return BadRequest();
                return Ok(originalCharacter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("charcters-original")]
        public async Task<IActionResult> GetCharactersOfOriginalServer([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
//                var id = User.FindFirst("ytfkugilkuh").Value;

                string userId = data.UserId;

                List<CharacterModel> charactersModel = data.Characters;
                List<CharacterModel> Sendlist = await _characterService.GetOriginalCharactersAsync(charactersModel,userId);
                return Ok(Sendlist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("charcters-my-server/{userId}/{spicies?}/{name?}")]
        public async Task<IActionResult> GetCharactersMyServer([FromRoute] string userId,[FromRoute] string? spicies, string? name) 
       {
            try
            {
                List<CharacterModel> Sendlist = await _characterService.GetMyCharactersAsync(userId, spicies,name);
                return Ok(Sendlist);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost("add-character")]
        public async Task<IActionResult> AddCharacter([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
                var userId = data.UserId;
                var character=data.Character;
                var original = data.OriginalId;
                var newCharacter=await _characterService.AddCharacterAsync(character,userId,original);
                if (newCharacter == null)
                    return Ok(null);
                return Ok(newCharacter);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("edit-character")]
        public async Task<IActionResult> EditCharacter([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
                var userId = data.UserId;
                var character = data.Character;
                var original = data.OriginalId;
                var newCharacter = await _characterService.EditCharacterAsync(character,userId);
                if (newCharacter == null)
                    return BadRequest();
                return Ok(newCharacter);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("delete-character")]
        public async Task<IActionResult> DeleteCharacter([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
                var userId = data.UserId;
                var character = data.Character;
                var result=await _characterService.RemoveCharacterAsync(character,userId);
                if (result)
                    return NoContent();
                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("reset-character")]
        public async Task<IActionResult> ResetCharacter([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
                var userId = data.UserId;
                var character = data.Character;
                if (character == null || userId == null)
                    return BadRequest();
                var result = await _characterService.ResetCharacterAsync(character, userId);
                if (result)
                    return Ok(character.OriginalId);
                return Ok(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("deleted-characters-id/{userId}")]
        public async Task<IActionResult> DeledesCharacters([FromRoute] string userId) //rick-morty server that changed
        {
            try
            {
                if (userId == null)
                    return BadRequest();
                var result = await _characterService.GetDeletedCharacterIdAsync(userId);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("reset-characters")]
        public async Task<IActionResult> ReseteCharacters([FromBody] GetCharacter data) //rick-morty server that changed
        {
            try
            {
                var characters = data.Characters;
                var userId = data.UserId;
                if (characters == null || userId == null)
                    return BadRequest();
                var returnCharacters = await _characterService.DeleteCharacterOriginalAsync(characters, userId);
                return Ok(returnCharacters);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public class GetCharacter
        {
            public List<CharacterModel> Characters { get; set; }
            public CharacterModel Character { get; set; }
            public string UserId { get; set; }
            public int OriginalId { get; set; }
        }

    }
}
