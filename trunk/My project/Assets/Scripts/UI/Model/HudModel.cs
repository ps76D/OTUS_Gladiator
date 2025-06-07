using System;
using System.Collections.Generic;
using GameEngine;
using GameEngine.CharacterSystem;
using GameEngine.CharacterSystem.StatsSystem;
using Infrastructure;
using UI.Infrastructure;
using UniRx;
using UnityEngine.Localization;

namespace UI.Model
{
    public class HudModel : IHudModel, IDisposable
    {
        private readonly UIManager _uiManager;
        private readonly DayService _dayService;
        private readonly MoneyStorage _moneyStorage;
        private readonly MoralService _moralService;
        private readonly CharacterService _characterService;
        
        public IReadOnlyReactiveProperty<int> DayCount => _dayCount;
        private readonly ReactiveProperty<int> _dayCount = new(0);
        
        public IReadOnlyReactiveProperty<int> MoneyCount => _moneyCount;
        private readonly ReactiveProperty<int> _moneyCount = new(0);
        
        /*public IReadOnlyReactiveProperty<int> AvailableActions => _availableActions;
        private readonly ReactiveProperty<int> _availableActions = new(0);
        public IReadOnlyReactiveProperty<int> MaxActionsCount => _maxActionsCount;
        private readonly ReactiveProperty<int> _maxActionsCount = new(0);*/
        
        public IReadOnlyReactiveProperty<int> LevelCount => _levelCount;
        private readonly ReactiveProperty<int> _levelCount = new(0);
        public IReadOnlyReactiveProperty<int> ExpCount => _expCount;
        private readonly ReactiveProperty<int> _expCount = new(0);
        public IReadOnlyReactiveProperty<int> RequiredExpCount => _requiredCount;
        private readonly ReactiveProperty<int> _requiredCount = new(0);
        
        public IReadOnlyReactiveProperty<CharacterStatsInfo> CharacterStatsInfo => _characterStatsInfo;
        private readonly ReactiveProperty<CharacterStatsInfo> _characterStatsInfo = new();
        
        public IReadOnlyReactiveProperty<bool> LevelUpButtonIsInteractable => _levelUpButtonIsInteractable;
        private readonly ReactiveProperty<bool> _levelUpButtonIsInteractable = new ();
        
        public IReadOnlyReactiveProperty<bool> ActionsButtonIsInteractable => _actionsButtonIsInteractable;
        private readonly ReactiveProperty<bool> _actionsButtonIsInteractable = new ();
        
        public IReadOnlyReactiveProperty<int> CurrentMoral => _currentMoral;
        private readonly ReactiveProperty<int> _currentMoral = new(0);
        
        private readonly List<IDisposable> _disposables = new();
        
        public HudModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            _dayService = uiManager.ProfileService.PlayerProfile.DayService;
            _moneyStorage = uiManager.ProfileService.PlayerProfile.MoneyStorage;
            ActionsService actionsService = uiManager.ProfileService.PlayerProfile.ActionsService;
            _moralService = uiManager.ProfileService.PlayerProfile.MoralService;
            CharacterService characterService = uiManager.ProfileService.PlayerProfile.CharacterService;
            _characterService = characterService;
            
            _dayService.OnDayChanged += UpdateDayCount;
            _moneyStorage.OnMoneyChanged += UpdateMoneyCount;
            characterService.CurrentCharacterProfile.CharacterLevel.OnExperienceChanged += UpdateExp;
            characterService.CurrentCharacterProfile.CharacterLevel.OnRequiredExperienceChanged += UpdateRequiredExp;
            characterService.CurrentCharacterProfile.CharacterLevel.OnLevelUp += UpdateLevel;
            
            var levelUpInteractableSubscription = characterService.CurrentCharacterProfile.CharacterLevel.CurrentExperience.
                Subscribe(count => _levelUpButtonIsInteractable.Value = characterService.CurrentCharacterProfile.CharacterLevel.CanLevelUp());
            _disposables.Add(levelUpInteractableSubscription);
            
            var actionsInteractableSubscription = actionsService.AvailableActions.
                Subscribe(x => _actionsButtonIsInteractable.Value = actionsService.CanSpendAction());
            _disposables.Add(actionsInteractableSubscription);
            
            var moralSubscription = _moralService.CurrentMoral.
                Subscribe(UpdateMoralState);
            _disposables.Add(moralSubscription);
            
            _characterStatsInfo.Value = characterService.CurrentCharacterProfile.CharacterStatsInfo;
                
            UpdateDayCount(_dayService.Day);
            UpdateMoneyCount(_moneyStorage.Money);
            UpdateLevel(characterService.CurrentCharacterProfile.CharacterLevel.CurrentLevel);
            UpdateExp(characterService.CurrentCharacterProfile.CharacterLevel.CurrentExperience.Value);
            UpdateRequiredExp(characterService.CurrentCharacterProfile.CharacterLevel.RequiredExperience);
            UpdateMoralState(_moralService.CurrentMoral.Value);
        }

        public void EndDay()
        {
            _dayService.NextDay();
            //TODO Перенести магические цифры в глобальный конфиг геймплея
            IncreaseMoral(1);
        }

        private void UpdateMoralState(int moral)
        {
            _currentMoral.Value = moral;
        }

        public LocalizedString GetMoralState()
        {
            return _moralService.GetMoralLevel().MoralLevelText;
        }
        
        public void IncreaseMoral(int moral)
        {
            _moralService.IncreaseMoral(moral);
        }
        
        public void DegreaseMoral(int moral)
        {
            _moralService.DegreaseMoral(moral);
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

        public void IncreaseStat(string statName)
        {
            var level = _characterService.CurrentCharacterProfile.CharacterLevel;
            
            //TODO Сделать расчет прироста опыта при прокачке
            level.AddExperience(25);
            
            CharacterStat stat = _characterStatsInfo.Value.GetStat(statName);
            
            //TODO Сделать прирост опыта стата зависящим от разных условий
            stat.AddStatExperience(1);

            stat.IncreaseValue();
            
            SpendAction(1);
            DegreaseMoral(1);
        }

        public void LevelUp()
        {
            var level = _characterService.CurrentCharacterProfile.CharacterLevel;
            level.LevelUp();
        }

        public void SpendAction(int action)
        {
            _uiManager.ProfileService.PlayerProfile.ActionsService.SpendAction(action);
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        public void Rest()
        {
            SpendAction(1);
            IncreaseMoral(5);
        }

        public void ShowMatchmakingScreen()
        {
            _uiManager.ShowMatchmakingScreen();
        }
    }
}