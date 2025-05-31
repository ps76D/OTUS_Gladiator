using System;
using System.Collections.Generic;
using GameEngine;
using GameEngine.CharacterSystem;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class CharacterSaveLoader : SaveLoader<CharacterService, CharacterData>
    {
        override protected CharacterData ConvertToData(CharacterService service)
        {
            Debug.Log($"<color=yellow>CharacterProfile convert to data = {service.CurrentCharacterProfile.CharacterInfo._guid}</color>");
            return new CharacterData
            {
                Guid = service.CurrentCharacterProfile.CharacterInfo._guid,
                Name = service.CurrentCharacterProfile.CharacterInfo._name,
                Description = service.CurrentCharacterProfile.CharacterInfo._description,
                
                CurrentLevel = service.CurrentCharacterProfile.CharacterLevel.CurrentLevel,
                CurrentExperience = service.CurrentCharacterProfile.CharacterLevel.CurrentExperience,
                
                Stats = service.CurrentCharacterProfile.CharacterStatsInfo.GetStats()
                /*Icon = service.CurrentCharacterProfile.CharacterInfo.Icon,*/
            };
        }

        override protected void SetupData(CharacterService service, CharacterData data)
        {
            service.SetupCharacter(data);
            Debug.Log($"<color=yellow>Setup CharacterProfile data = {service.CurrentCharacterProfile.CharacterInfo._guid}</color>");
        }

        /*override protected void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }*/
    }
}
