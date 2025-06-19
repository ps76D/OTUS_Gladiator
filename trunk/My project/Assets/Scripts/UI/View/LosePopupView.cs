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
    public class LosePopupView : UIScreen
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TMP_Text _moralChanged;
        
        private ILosePopupModel _viewModel;
        public ILosePopupModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(ILosePopupModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);

            _nextButton.onClick.AddListener(BackToTraining);
            
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
        
        private void UpdateMoralLabel(string text)
        {
            _moralChanged.text = text;
        }
    }
}