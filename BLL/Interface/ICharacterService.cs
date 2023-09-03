using BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICharacterService
    {
        Task<CharacterModel> GetCharacterAsync(int CharacterId, string userId);
        Task<CharacterModel> AddCharacterAsync(CharacterModel character, string userId, int originalId);
        Task<List<CharacterModel>> GetOriginalCharactersAsync(List<CharacterModel> charactersModel, string userId);
        Task<List<CharacterModel>> GetMyCharactersAsync(string userId, string spicies,string name);
        Task<CharacterModel> EditCharacterAsync(CharacterModel characterModel, string userId);
        Task<bool> RemoveCharacterAsync(CharacterModel characterModel, string userId);
        Task<bool> ResetCharacterAsync(CharacterModel characterModel, string userId);
        Task<List<int>> GetDeletedCharacterIdAsync(string userId);
        Task<List<CharacterModel>> DeleteCharacterOriginalAsync(List<CharacterModel> originalCharacterList, string userId);

    }
}
