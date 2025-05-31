using System;
using System.Collections.Generic;
using TMPro;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class Hud : UIScreen
    {
        [SerializeField] private Button _endDayButton;
        [SerializeField] private Button _inGameMenuButton;
        
        [SerializeField] private TMP_Text _currentDayText;
        [SerializeField] private TMP_Text _moneyCountText;
        [SerializeField] private TMP_Text _levelCountText;
        [SerializeField] private TMP_Text _expCountText;
        [SerializeField] private Slider _expSlider;
        
        [SerializeField] private StatsView _statsView;
        
        /*[SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _saveGameButton;
        */
        
        private readonly List<IDisposable> _disposables = new();
        
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

            _endDayButton.onClick.AddListener(EndDayButtonClicked);
            _inGameMenuButton.onClick.AddListener(InGameMenuButtonClicked);
            
            _statsView.Show(new StatsModel(_uiManager));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _endDayButton.onClick.RemoveListener(EndDayButtonClicked);
            _inGameMenuButton.onClick.RemoveListener(InGameMenuButtonClicked);
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            _statsView.Hide();
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
            _expCountText.text = viewModel.ExpCount + " / " + _viewModel.RequiredExpCount;
            
            float sliderValue = (float)viewModel.ExpCount.Value / _viewModel.RequiredExpCount.Value;
            
            _expSlider.value = sliderValue;
        }
        
        private void UpdateLevelText(int exp)
        {
            SetLevelData(_viewModel);
        }
        
        private void SetLevelData(IHudModel viewModel)
        {
            _levelCountText.text = viewModel.LevelCount.ToString();
        }
        
        private void InGameMenuButtonClicked()
        {
            _viewModel.InGameMenuShow();
        }
    }
}