using System;
using GameEngine.ActionsSystem;
using GameEngine.DaySystem;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public class CharacterProfile : IDisposable
    {
        [SerializeField] private ActionsService _actionsService;

        [SerializeField] private CharacterInfo _characterInfo;
        [SerializeField] private CharacterLevel _characterLevel;

        [SerializeField] private CharacterStatsInfo _characterStatsInfo;
        public ActionsService ActionsService => _actionsService;
        public CharacterInfo CharacterInfo => _characterInfo;
        public CharacterLevel CharacterLevel => _characterLevel;
        
        public CharacterStatsInfo CharacterStatsInfo => _characterStatsInfo;

        public CharacterProfile(CharacterInfoSObj characterInfoSObj, DayService dayService)
        {
            _characterInfo = new CharacterInfo
            {
                _guid = characterInfoSObj.CharacterGuid,
                _name = characterInfoSObj.CharacterName,
                _description = characterInfoSObj.CharacterDescription,
                /*_icon = characterInfoSObj.CharacterIcon*/
            };

            /*_characterInfo = CreateCharacterInfo(characterInfoData);*/
            _characterInfo._icon = characterInfoSObj.CharacterIcon;
            _characterInfo._battleImage = characterInfoSObj.CharacterBattleImage;
            _characterLevel = new CharacterLevel();
            
            _characterStatsInfo = CreateCharacterStatsInfo(characterInfoSObj);

            _actionsService = new ActionsService(dayService);
            _actionsService.BaseMaxActionsCount = characterInfoSObj.MaxActionsCount;

            _actionsService.OnCalcMaxActionsCount += CalcMaxActions;
        }

        private CharacterStatsInfo CreateCharacterStatsInfo(CharacterInfoSObj characterInfoData)
        {
            CharacterStatsInfo characterStatsInfo = new ();
                
            int index;
            for (index = 0; index < characterInfoData.StatsDatabase.StartStatsDatabase.Count; index++)
            {
                StatData statData = characterInfoData.StatsDatabase.StartStatsDatabase[index];
                CharacterStat stat = new()
                {
                    Name = statData._statInfoData.StatName,
                };

                stat.ChangeValue(statData._startStatValue);

                characterStatsInfo.AddStat(stat);
            }

            return characterStatsInfo;
        }

        public void CalcMaxActions()
        {
            var actionsBoost = _characterStatsInfo.GetStat(StatsNamesConstants.Endurance).Value / 5;
            
            Debug.Log("Calc Actions Boost: " + actionsBoost);

            _actionsService.MaxActionsCount.Value = _actionsService.CalcMaxActionsCount(_actionsService.BaseMaxActionsCount, actionsBoost);
        }
        
        public void Dispose()
        {
            _actionsService.OnCalcMaxActionsCount -= CalcMaxActions;
            
            if (_actionsService != null)
            {
                (_actionsService as IDisposable)?.Dispose();
            }
            
            Debug.Log("Dispose ActionsService");
        }
    }
}