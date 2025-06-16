using System;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsView : UIScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _fadeCloseButton;
        
        private ISettingsModel _viewModel;
        public ISettingsModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(ISettingsModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _backButton.onClick.AddListener(BackToInGameMenu);
            _fadeCloseButton.onClick.AddListener(BackToInGameMenu);
        }
        
        public void Close()
        {            
            _backButton.onClick.RemoveListener(BackToInGameMenu);
            _fadeCloseButton.onClick.RemoveListener(BackToInGameMenu);

            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            gameObject.SetActive(false);
        }

        private void BackToInGameMenu()
        {
            Close();
        }
    }
}