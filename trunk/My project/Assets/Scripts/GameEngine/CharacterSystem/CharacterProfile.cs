using System;
using GameEngine.CharacterSystem.StatsSystem;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public class CharacterProfile
    {
        /*[SerializeField] private CharacterInfoData _characterInfoData;*/
        [SerializeField] private CharacterInfo _characterInfo;
        [SerializeField] private CharacterLevel _characterLevel;

        [SerializeField] private CharacterStatsInfo _characterStatsInfo;

        public CharacterInfo CharacterInfo => _characterInfo;
        public CharacterLevel CharacterLevel => _characterLevel;
        
        public CharacterStatsInfo CharacterStatsInfo => _characterStatsInfo;

        /*public CharacterInfoData CharacterInfoData => _characterInfoData; */

        public CharacterProfile(CharacterInfoSObj characterInfoSObj)
        {
            _characterInfo = new CharacterInfo
            {
                _guid = characterInfoSObj.CharacterGuid,
                _name = characterInfoSObj.CharacterName,
                _description = characterInfoSObj.CharacterDescription,
                /*_icon = characterInfoSObj.CharacterIcon*/
            };

            /*_characterInfo = CreateCharacterInfo(characterInfoData);*/
            
            _characterLevel = new CharacterLevel();
            
            _characterStatsInfo = CreateCharacterStatsInfo(characterInfoSObj);
        }

        /*private CharacterInfo CreateCharacterInfo(CharacterInfoData characterInfoData)
        {
            var characterInfo = new CharacterInfo();

            characterInfo.ChangeName(characterInfoData.CharacterName);
            characterInfo.ChangeDescription(characterInfoData.CharacterDescription);
            characterInfo.ChangeIcon(characterInfoData.CharacterIcon);
            
            return characterInfo;
        }*/

        private CharacterStatsInfo CreateCharacterStatsInfo(CharacterInfoSObj characterInfoData)
        {
            CharacterStatsInfo characterStatsInfo = new ();

            /*characterStatsInfo.SetStats(characterInfoData.StatsDatabase);*/
                
            int index;
            for (index = 0; index < characterInfoData.StatsDatabase.StartStatsDatabase.Count; index++)
            {
                StatData statData = characterInfoData.StatsDatabase.StartStatsDatabase[index];
                CharacterStat stat = new()
                {
                    Name = statData._statInfoData.StatName,
                };

                stat.ChangeValue(statData._startStatValue);
                /*stat.ChangeIcon(statData._statInfoData.StatIcon);*/

                characterStatsInfo.AddStat(stat);
            }

            return characterStatsInfo;
        }
    }
}