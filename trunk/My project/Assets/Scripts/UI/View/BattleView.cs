using System;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;

namespace UI
{
    public class BattleView : UIScreen
    {
        private IBattleModel _viewModel;
        public IBattleModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(IBattleModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            /*Cleanup();
            
            SetupEnemyWidgets(_viewModel);
            SetupPlayerWidget(_viewModel);
            
            _backButton.onClick.AddListener(Close);
            _fadeCloseButton.onClick.AddListener(Close);
            _toBattleButton.onClick.AddListener(ToBattle);*/
        }

        private void OnDisable()
        {
            /*Cleanup();
            
            _backButton.onClick.RemoveListener(Close);*/
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
