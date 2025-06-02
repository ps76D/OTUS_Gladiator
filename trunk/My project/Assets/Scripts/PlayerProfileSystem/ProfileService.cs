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
        private MoralService _moralService;
        [Inject]
        private CharacterService _characterService;

        [Inject]
        [SerializeField] private PlayerProfile _playerProfile;
        
        [Inject]
        private PlayerProfileDefault _playerProfileDefault;
        
        [Inject]
        private MoralConfig _moralConfig;
        
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
            _playerProfile = new PlayerProfile(_moneyStorage, _dayService, _actionsService, _characterService, _moralService);
            _playerProfile.MoneyStorage.SetupMoney(_playerProfileDefault.Money);
            _playerProfile.DayService.SetupDay(1);
            _playerProfile.ActionsService.SetupMaxActionsCount(_playerProfileDefault.MaxActionsCount);
            _playerProfile.ActionsService.RecoverAllActions();
            _playerProfile.CharacterService.CreateCharacter(_playerProfile.CharacterService.CharacterInfoDefault);
            _playerProfile.MoralService.SetupMaxMoralCount(_moralConfig._moralLevels.Find(x => x.name == MoralLevelNamesConstants.Courage).MoralLevelValue);
            _playerProfile.MoralService.SetupCurrentMorals(_moralConfig._moralLevels.Find(x => x.name == MoralLevelNamesConstants.High).MoralLevelValue + 5);
        }
    }
}