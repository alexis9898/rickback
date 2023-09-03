using AutoMapper;
using BLL.Interface;
using BLL.Model;
using DAL.Interface;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public CharacterService(ICharacterRepository characterRepository, IMapper mapper)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task<CharacterModel> GetCharacterAsync(int CharacterId, string userId)
        {
            if(CharacterId == 0)
                return null;
            var character = await _characterRepository.GetCharacterOriginalAsync(CharacterId, userId);
            if (character == null)
                return null;
            return _mapper.Map<CharacterModel>(character);
        }

        public async Task<List<CharacterModel>> GetMyCharactersAsync(string userId,string spicies, string name)
        {
            var characters = await _characterRepository.GetCharactersAsync(userId, spicies, name);
            return _mapper.Map<List<CharacterModel>>(characters);
        }

        public async Task<List<CharacterModel>> GetOriginalCharactersAsync(List<CharacterModel> charactersModel, string userId)
        {
            List<CharacterModel> Sendlist = new List<CharacterModel>();
            for (int i = 0; i < charactersModel.Count; i++)
            {
                CharacterModel character = await GetCharacterAsync(charactersModel[i].Id, userId);
                if (character == null)
                {
                    charactersModel[i].OriginalId = charactersModel[i].Id;
                    Sendlist.Add(charactersModel[i]);
                    continue;
                }
                if (character.IsDeleted)
                    continue;
                //character.Id= (int)character.OriginalId;
                character.Episode = charactersModel[i].Episode;
                Sendlist.Add(character);
            }
            return Sendlist;
        }

        public async Task<CharacterModel> AddCharacterAsync(CharacterModel character, string userId, int originalId)
        {
            character.IsDeleted = false;
            if (userId == null || userId == "")
                return null;
            character.UserId = userId;
            if (originalId != 0)
            {
                var hasCharacter = await _characterRepository.GetCharacterOriginalAsync(originalId, userId); // if already have thr original
                if (hasCharacter != null)
                    return null;
                character.OriginalId = originalId;
            } else
                character.OriginalId = null;

            var newCharacter=_mapper.Map<Character>(character);
            newCharacter = await _characterRepository.AddCharacterAsync(newCharacter);

            return _mapper.Map<CharacterModel>(newCharacter);
        }

        public async Task<CharacterModel> EditCharacterAsync(CharacterModel characterModel, string userId)
        {
            //var character=await

            characterModel.UserId=userId;
            characterModel.IsDeleted=false;
            int originalId = (characterModel.OriginalId==null)?0:(int)characterModel.OriginalId;
            if (originalId != 0)
            {
                var hasCharacter = await _characterRepository.GetCharacterOriginalAsync(originalId, userId); // if already have thr original
                if (hasCharacter == null)
                {
                    characterModel.Id = 0;
                    var addCharacter= await AddCharacterAsync(characterModel, userId, originalId);
                    return addCharacter;
                }
                //hasCharacter=_mapper.Map<Character>(characterModel);    
                hasCharacter.Status=characterModel.Status;
                hasCharacter.Species=characterModel.Species;
                hasCharacter.Image=characterModel.Image;
                hasCharacter.Name=characterModel.Name;
                hasCharacter.Location=characterModel.Location;
                await _characterRepository.UpdateCharacterAsync(hasCharacter);
                return _mapper.Map<CharacterModel>(hasCharacter);
            }
            characterModel.OriginalId = null;
            Character editCharacter = await _characterRepository.GetMyCharacterAsync(characterModel.Id, userId);
            if(editCharacter == null)
                return null;
            editCharacter.Status = characterModel.Status;
            editCharacter.Species = characterModel.Species;
            editCharacter.Image = characterModel.Image;
            editCharacter.Name = characterModel.Name;
            editCharacter.Location = characterModel.Location;

            //editCharacter = _mapper.Map<Character>(characterModel);
            await _characterRepository.UpdateCharacterAsync(editCharacter);
            return _mapper.Map<CharacterModel>(editCharacter);
        }
        public async Task<bool> RemoveCharacterAsync(CharacterModel characterModel, string userId)
        {
            if (characterModel == null)
                return false;
            if(characterModel.OriginalId==0 || characterModel.OriginalId == null) //user character
            {
                var character = await _characterRepository.GetMyCharacterAsync(characterModel.Id, userId);
                if (character == null)
                    return false;
                 await _characterRepository.DeleteCharacterAsync(character);
                return true;
            }
            // original character
            var originalCharacter = await _characterRepository.GetCharacterOriginalAsync((int)characterModel.OriginalId, userId);
            if(originalCharacter == null) //if not exist in dataBase
            {
                characterModel.UserId = userId;
                characterModel.Id=0;
                characterModel.IsDeleted = true; 
                await _characterRepository.AddCharacterAsync(_mapper.Map<Character>(characterModel));
                return true;
            }
            originalCharacter.IsDeleted = true;
            originalCharacter.UserId = userId;
            await _characterRepository.UpdateCharacterAsync(_mapper.Map<Character>(originalCharacter));
            return true;
        }

        public async Task<bool> ResetCharacterAsync(CharacterModel characterModel, string userId)
        {
            //var character=await GetCharacterAsync((int)characterModel.OriginalId, userId);
            var character = await _characterRepository.GetCharacterOriginalAsync((int)characterModel.OriginalId, userId);
            if (character == null)
                return false;
            await _characterRepository.DeleteCharacterAsync(_mapper.Map<Character>(character));
            return true;
        }

        public async Task<List<int>> GetDeletedCharacterIdAsync( string userId)
        {
            var characters=await _characterRepository.GetDeletedCharacterAsync(userId);
            List<int> result=new List<int>();
            foreach (var character in characters)
                result.Add((int)character.OriginalId);
            return result;
        }
        public async Task<List<CharacterModel>> DeleteCharacterOriginalAsync(List<CharacterModel> originalCharacterList,string userId)
        {
            List<CharacterModel> result=new List<CharacterModel>();
            for (int i = 0; i < originalCharacterList.Count; i++)
            {
                if (originalCharacterList[i]==null)
                    continue;
                var character = await _characterRepository.GetCharacterOriginalAsync(originalCharacterList[i].Id, userId);
                if (character == null)
                    continue;

                await _characterRepository.DeleteCharacterAsync(_mapper.Map<Character>(character));
                originalCharacterList[i].OriginalId=originalCharacterList[i].Id;
                result.Add(originalCharacterList[i]);
            }
            return result;
        }

    }
}
