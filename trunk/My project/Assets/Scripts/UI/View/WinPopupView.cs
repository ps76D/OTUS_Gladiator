using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinPopupView : UIScreen
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TMP_Text _moneyReward;
        [SerializeField] private TMP_Text _moralChanged;
        
        private IWinPopupModel _viewModel;
        public IWinPopupModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(IWinPopupModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _nextButton.onClick.AddListener(BackToTraining);

            _disposables.Add(_viewModel.RewardCount.Subscribe(UpdateRewardCount));
            _disposables.Add(_viewModel.MoralLevelChanged.Subscribe(UpdateMoralLabel));
        }
        
        public void Close()
        {            
            _nextButton.onClick.RemoveListener(BackToTraining);

            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            gameObject.SetActive(false);
        }
        
        private void BackToTraining()
        {
            _viewModel.BackToTraining();
            Close();
        }

        private void UpdateRewardCount(int count)
        {
            _moneyReward.text = count.ToString();
        }
        
        private void UpdateMoralLabel(string text)
        {
            _moralChanged.text = text;
        }
    }
}