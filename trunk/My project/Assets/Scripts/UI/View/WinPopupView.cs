using System;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinPopupView : UIScreen
    {
        [SerializeField] private Button _nextButton;
        
        private IWinPopupModel _viewModel;
        public IWinPopupModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(IWinPopupModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _nextButton.onClick.AddListener(BackToTraining);
        }
        
        public void Close()
        {            
            _nextButton.onClick.RemoveListener(BackToTraining);

            foreach (var disposable in _disposables)
                disposable.Dispose();

            /*_viewModel.Dispose();*/
            
            gameObject.SetActive(false);
        }
        
        private void BackToTraining()
        {
            _viewModel.BackToTraining();
            Close();
            /*_uiManager.HideWinPopup();*/
        }
    }
}