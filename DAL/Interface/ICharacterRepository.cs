using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICharacterRepository
    {
        Task<Character> GetCharacterOriginalAsync(int characterId, string userId);
        Task<List<Character>> GetCharactersAsync(string userId,string spicies, string name);
        Task<Character> AddCharacterAsync(Character character);
        Task<Character> GetMyCharacterAsync(int characterId, string userId);
        Task<List<Character>> GetDeletedCharacterAsync(string userId);


        Task UpdateCharacterAsync(Character character);
        Task DeleteCharacterAsync(Character character);


    }
}
