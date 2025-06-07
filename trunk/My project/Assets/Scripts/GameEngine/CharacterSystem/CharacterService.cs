using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterService
    {
        [Inject]
        private CharacterDatabase _characterDatabase;
        [Inject]
        private CharacterInfoSObj _characterInfoDefault;
        
        private CharacterInfoSObj _currentCharacterInfoSObj;
        
        public event Action<CharacterProfile> OnCharacterDataChanged;

        public CharacterProfile CurrentCharacterProfile => _currentCharacterProfile;
        public CharacterInfoSObj CharacterInfoDefault => _characterInfoDefault;
        public CharacterInfoSObj CurrentCharacterInfoSObj => _currentCharacterInfoSObj;
        
        [SerializeField] private CharacterProfile _currentCharacterProfile;

        public void SetupCharacter(CharacterData characterData)
        {
            _currentCharacterProfile.CharacterInfo._guid = characterData.Guid;
            _currentCharacterProfile.CharacterInfo._name = characterData.Name;
            _currentCharacterProfile.CharacterInfo._description = characterData.Description;
            
            _currentCharacterProfile.CharacterLevel.CurrentLevel = characterData.CurrentLevel;
            _currentCharacterProfile.CharacterLevel.CurrentExperience.Value = characterData.CurrentExperience;
            
            
            _currentCharacterInfoSObj = _characterDatabase.CharacterInfoDatabaseSObjs.
                Find(c => c.CharacterGuid == _currentCharacterProfile.CharacterInfo._guid);
            _currentCharacterProfile.CharacterInfo._icon = _currentCharacterInfoSObj.CharacterIcon;
            
            _currentCharacterProfile.CharacterStatsInfo.SetStats(characterData);
            
            _currentCharacterProfile.CharacterInfo._icon = _currentCharacterInfoSObj.CharacterIcon;
            
            OnCharacterDataChanged?.Invoke(_currentCharacterProfile);
        }
        
        [Button]
        public void CreateCharacter(CharacterInfoSObj characterInfo)
        {
            _currentCharacterProfile = new CharacterProfile(characterInfo);
            
            OnCharacterDataChanged?.Invoke(_currentCharacterProfile);
        }
    }
}