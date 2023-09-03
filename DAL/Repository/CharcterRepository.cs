using DAL.Data;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CharcterRepository : ICharacterRepository
    {
        private readonly AppDataContext _context;
        public CharcterRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<Character> GetCharacterOriginalAsync(int characterId, string userId)
        {
            var character = await _context.Characters.Where(x => x.OriginalId == characterId && x.UserId == userId).FirstOrDefaultAsync();
            if (character == null)
                return null;
            return character;
        }

        public async Task<Character> GetMyCharacterAsync(int characterId, string userId)
        {
            var character = await _context.Characters.Where(x => x.Id == characterId && x.UserId == userId ).FirstOrDefaultAsync();
            if (character == null)
                return null;
            return character;
        }

        //all-deleted-original-characters
        public async Task<List<Character>> GetDeletedCharacterAsync(string userId)
        {
            var characters = await _context.Characters.Where(x =>x.OriginalId!=null && x.OriginalId!=0 && x.UserId == userId && x.IsDeleted==true).ToListAsync();
            return characters;
        }

        //my characters
        //public async Task<List<Character>> GetCharactersAsync(string userId, string spicies)
        //{
        //    var characters = await _context.Characters.Where(x=> x.UserId == userId && x.IsDeleted==false && (x.OriginalId==null || x.OriginalId==0) && (spicies== "undefined" || spicies == null || x.Species == spicies)).ToListAsync(); 
        //    return characters;
        //}

        public async Task<List<Character>> GetCharactersAsync(string userId, string spicies, string name)
        {
            var characters = await _context.Characters.Where(
                x => x.UserId == userId && x.IsDeleted == false &&
                (x.OriginalId == null || x.OriginalId == 0)
                //&& ((spicies != "undefined" || spicies != null ) && x.Species == spicies) 
                && ((spicies != "undefined" && spicies != "null" )?x.Species == spicies:x.Species!=null)
                && ((name != "undefined" && name != "null" )?(x.Name.ToLower().Contains(name.ToLower())): x.Name!=null)
                // && (name == "undefined" || name == null || x.Name == name )
                )
                .ToListAsync();
            return characters;
        }

        //add
        public async Task<Character> AddCharacterAsync(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return character;
        }

        // PUT/PATCH 
        public async Task UpdateCharacterAsync(Character character)
        {

            //_context.Images.Update(film); ????
            _context.Characters.Update(character);
            await _context.SaveChangesAsync();
            return;
        }

        //delete
        public async Task DeleteCharacterAsync(Character character)
        {
            _context.Remove(character);
            await _context.SaveChangesAsync();
        }
    }
}
