using System;
using GameEngine;
using GameEngine.CharacterSystem;
using UnityEngine;
using Zenject;

namespace PlayerProfileSystem
{
    [Serializable]
    public class ProfileService
    {
        [Inject]
        private MoneyStorage _moneyStorage;
        [Inject]
        private DayService _dayService;
        [Inject]
        private ActionsService _actionsService;
        [Inject]
        private CharacterService _characterService;
        
        [Inject]
        [SerializeField] private PlayerProfile _playerProfile;
        
        [Inject]
        private PlayerProfileDefault _playerProfileDefault;
        
        public PlayerProfile PlayerProfile
        {
            get => _playerProfile;
            set => _playerProfile = value;
        }
        
        public ProfileService()
        {
            ProfileDebuggerInitialize();
        }

        private void ProfileDebuggerInitialize()
        {
#if UNITY_EDITOR
            new GameObject("Profile Debugger").AddComponent<ProfileDebugger>().Manager = this;
#endif
        }

        public void InitializeNewGameProfile()
        {
            _playerProfile = new PlayerProfile(_moneyStorage, _dayService, _actionsService, _characterService);
            _playerProfile.MoneyStorage.SetupMoney(_playerProfileDefault.Money);
            _playerProfile.DayService.SetupDay(1);
            _playerProfile.ActionsService.SetupMaxActionsCount(_playerProfileDefault.MaxActionsCount);
            _playerProfile.ActionsService.RecoverAllActions();
            _playerProfile.CharacterService.CreateCharacter(_playerProfile.CharacterService.CharacterInfoDefault);
        }
    }
}