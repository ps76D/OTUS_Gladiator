using System;
using System.Collections.Generic;
using GameEngine.CharacterSystem.StatsSystem;
using TMPro;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Hud : UIScreen
    {
        [SerializeField] private Button _endDayButton;
        [SerializeField] private Button _inGameMenuButton;
        [SerializeField] private Button _trainStatStrengthButton;
        [SerializeField] private Button _trainStatEnduranceButton;
        [SerializeField] private Button _trainStatAgilityButton;
        [SerializeField] private Button _restButton;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _matchmakingButton;
        
        [SerializeField] private TMP_Text _currentDayText;
        [SerializeField] private TMP_Text _moneyCountText;
        [SerializeField] private TMP_Text _levelCountText;
        [SerializeField] private TMP_Text _expCountText;
        [SerializeField] private TMP_Text _currentMoralText;
        [SerializeField] private Slider _expSlider;
        
        [SerializeField] private StatsView _statsView;
        [SerializeField] private ActionsPanelView _actionsPanelView;
        
        private readonly List<IDisposable> _disposables = new();
        
        public Action OnStrengthIncreased;
        public Action OnEnduranceIncreased;
        public Action OnAgilityIncreased;
        public Action OnLevelUp;
        
        public Action OnMoralChanged;
        
        
        
        
        private IHudModel _viewModel;
        public IHudModel ViewModel => _viewModel;

        public void Show(IHudModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);

            _disposables.Add(viewModel.DayCount.Subscribe(UpdateDayText));
            _disposables.Add(viewModel.MoneyCount.Subscribe(UpdateMoneyText));
            _disposables.Add(viewModel.ExpCount.Subscribe(UpdateExpText));
            _disposables.Add(viewModel.LevelCount.Subscribe(UpdateLevelText));
            _disposables.Add(viewModel.CurrentMoral.Subscribe(UpdateMoralText));
            _disposables.Add(viewModel.LevelUpButtonIsInteractable.SubscribeToInteractable(_levelUpButton));
            _disposables.Add(viewModel.ActionsButtonIsInteractable.SubscribeToInteractable(_trainStatStrengthButton));
            _disposables.Add(viewModel.ActionsButtonIsInteractable.SubscribeToInteractable(_trainStatEnduranceButton));
            _disposables.Add(viewModel.ActionsButtonIsInteractable.SubscribeToInteractable(_trainStatAgilityButton));
            _disposables.Add(viewModel.ActionsButtonIsInteractable.SubscribeToInteractable(_restButton));

            _endDayButton.onClick.AddListener(EndDayButtonClicked);
            _inGameMenuButton.onClick.AddListener(InGameMenuButtonClicked);
            _trainStatStrengthButton.onClick.AddListener(TrainStatStrength);
            _trainStatEnduranceButton.onClick.AddListener(TrainStatEndurance);
            _trainStatAgilityButton.onClick.AddListener(TrainStatAgility);
            _levelUpButton.onClick.AddListener(LevelUpButtonClicked);
            _restButton.onClick.AddListener(RestButtonClicked);
            
            _matchmakingButton.onClick.AddListener(MatchmakingButtonClicked);

            _statsView.Show(new StatsModel(_uiManager));

            _actionsPanelView.Show(new ActionsPanelModel(_uiManager));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _endDayButton.onClick.RemoveListener(EndDayButtonClicked);
            _inGameMenuButton.onClick.RemoveListener(InGameMenuButtonClicked);
            _trainStatStrengthButton.onClick.RemoveListener(TrainStatStrength);
            _trainStatEnduranceButton.onClick.RemoveListener(TrainStatEndurance);
            _trainStatAgilityButton.onClick.RemoveListener(TrainStatAgility);
            _matchmakingButton.onClick.RemoveListener(MatchmakingButtonClicked);
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            _statsView.Hide();
            _actionsPanelView.Hide();
        }

        private void MatchmakingButtonClicked()
        {
            _viewModel.ShowMatchmakingScreen();
        }

        private void EndDayButtonClicked()
        {
            EndDay();
        }
        
        private void EndDay()
        {
            _viewModel.EndDay();
        }

        private void UpdateDayText(int day)
        {
            SetDayData(_viewModel);
        }
        
        private void SetDayData(IHudModel viewModel)
        {
            _currentDayText.text = viewModel.DayCount.ToString();
        }
        
        private void UpdateMoneyText(int money)
        {
            SetMoneyData(_viewModel);
        }
        private void SetMoneyData(IHudModel viewModel)
        {
            _moneyCountText.text = viewModel.MoneyCount.ToString();
        }
        
        private void UpdateExpText(int exp)
        {
            SetExpData(_viewModel);
        }
        
        private void SetExpData(IHudModel viewModel)
        {
            _expCountText.text = viewModel.ExpCount + " / " + viewModel.RequiredExpCount;
            
            float sliderValue = (float)viewModel.ExpCount.Value / viewModel.RequiredExpCount.Value;
            
            _expSlider.value = sliderValue;
        }
        
        private void UpdateLevelText(int exp)
        {
            SetLevelData(_viewModel);
            OnLevelUp?.Invoke();
        }
        
        private void SetLevelData(IHudModel viewModel)
        {
            _levelCountText.text = viewModel.LevelCount.ToString();
        }
        
        private void InGameMenuButtonClicked()
        {
            _viewModel.InGameMenuShow();
        }

        private void TrainStat(IHudModel viewModel, string statName)
        {
            viewModel.IncreaseStat(statName);
            _actionsPanelView.UpdateActionsPanel(_uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions.Value);
        }
        
        private void TrainStatStrength()
        {
            TrainStat(_viewModel, StatsNamesConstants.Strength);
            OnStrengthIncreased?.Invoke();
        }
        private void TrainStatEndurance()
        {
            TrainStat(_viewModel, StatsNamesConstants.Endurance);
            OnEnduranceIncreased?.Invoke();
        }
        private void TrainStatAgility()
        {
            TrainStat(_viewModel, StatsNamesConstants.Agility);
            OnAgilityIncreased?.Invoke();
        }
        private void LevelUpButtonClicked()
        {
            /*_viewModel.LevelUp();*/
            //TODO сделать вызов окна распределения очков перков
        }

        private void UpdateMoralText(int moral)
        {
            UpdateMoralData(_viewModel);
            /*OnMoralChanged?.Invoke();*/
        }
        private void UpdateMoralData(IHudModel viewModel)
        {
            _currentMoralText.text = viewModel.GetMoralState().GetLocalizedString();
        }

        private void RestButtonClicked()
        {
            _viewModel.Rest();
            OnMoralChanged?.Invoke();
        }
    }
}