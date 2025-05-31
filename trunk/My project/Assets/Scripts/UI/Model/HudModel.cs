using GameEngine;
using GameEngine.CharacterSystem;
using Infrastructure;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class HudModel : IHudModel
    {
        private readonly UIManager _uiManager;
        private readonly DayService _dayService;
        private readonly MoneyStorage _moneyStorage;
        
        public IReadOnlyReactiveProperty<int> DayCount => _dayCount;
        private readonly ReactiveProperty<int> _dayCount = new(0);
        
        public IReadOnlyReactiveProperty<int> MoneyCount => _moneyCount;
        private readonly ReactiveProperty<int> _moneyCount = new(0);
        
        public IReadOnlyReactiveProperty<int> LevelCount => _levelCount;
        private readonly ReactiveProperty<int> _levelCount = new(0);
        public IReadOnlyReactiveProperty<int> ExpCount => _expCount;
        private readonly ReactiveProperty<int> _expCount = new(0);
        public IReadOnlyReactiveProperty<int> RequiredExpCount => _requiredCount;
        private readonly ReactiveProperty<int> _requiredCount = new(0);
        
        
        /*private readonly List<IDisposable> _disposables = new();*/
        
        public HudModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            _dayService = uiManager.ProfileService.PlayerProfile.DayService;
            _moneyStorage = uiManager.ProfileService.PlayerProfile.MoneyStorage;
            CharacterService characterService = uiManager.ProfileService.PlayerProfile.CharacterService;
            
            _dayService.OnDayChanged += UpdateDayCount;
            _moneyStorage.OnMoneyChanged += UpdateMoneyCount;
            characterService.CurrentCharacterProfile.CharacterLevel.OnExperienceChanged += UpdateExp;
            characterService.CurrentCharacterProfile.CharacterLevel.OnRequiredExperienceChanged += UpdateRequiredExp;
            characterService.CurrentCharacterProfile.CharacterLevel.OnLevelUp += UpdateLevel;
                
            UpdateDayCount(_dayService.Day);
            UpdateMoneyCount(_moneyStorage.Money);
            UpdateLevel(characterService.CurrentCharacterProfile.CharacterLevel.CurrentLevel);
            UpdateExp(characterService.CurrentCharacterProfile.CharacterLevel.CurrentExperience);
            UpdateRequiredExp(characterService.CurrentCharacterProfile.CharacterLevel.RequiredExperience);
        }

        public void EndDay()
        {
            _dayService.NextDay();
        }

        private void UpdateDayCount(int day)
        {
            _dayCount.Value = day;
        }
        
        private void UpdateMoneyCount(int money)
        {
            _moneyCount.Value = money;
        }
        
        public void InGameMenuShow()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<PauseState>();
        }
        
        private void UpdateLevel(int level)
        {
            _levelCount.Value = level;
        }
        
        private void UpdateExp(int experience)
        {
            _expCount.Value = experience;
        }
        
        private void UpdateRequiredExp(int experience)
        {
            _requiredCount.Value = experience;
        }
        
        /*public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }*/
    }
}