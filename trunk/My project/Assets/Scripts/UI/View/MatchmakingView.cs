using System;
using System.Collections.Generic;
using GameEngine.CharacterSystem;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class MatchmakingView : UIScreen
    {
        [SerializeField] private Button _toBattleButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _fadeCloseButton;

        [SerializeField] private UnitWidget _unitWidgetPrefab;
        
        [SerializeField] private List<UnitWidget> _enemyWidgets = new();
        [SerializeField] private UnitWidget _currentlySelectedWidget;
        
        [SerializeField] private UnitWidget _playerWidget;
        
        [SerializeField] private ScrollRect _matchmakingScrollView;
        [SerializeField] private Transform _scrollViewRoot;
        
        [SerializeField] private CharacterInfoSObj _opponentInfoSObj;
        
        private readonly List<IDisposable> _disposables = new();
        
        private IMatchmakingModel _viewModel;
        public IMatchmakingModel ViewModel => _viewModel;

        public void Show(IMatchmakingModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            Cleanup();
            
            SetupEnemyWidgets(_viewModel);
            SetupPlayerWidget(_viewModel);
            
            _backButton.onClick.AddListener(Close);
            _fadeCloseButton.onClick.AddListener(Close);
            _toBattleButton.onClick.AddListener(ToBattle);
            
            _disposables.Add(_viewModel.IsOpponentSelected.SubscribeToInteractable(_toBattleButton));
        }

        private void Close()
        {            
            Cleanup();
            
            _backButton.onClick.RemoveListener(Close);
            _fadeCloseButton.onClick.RemoveListener(Close);
            _toBattleButton.onClick.RemoveListener(ToBattle);

            _viewModel.Cleanup();
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            gameObject.SetActive(false);
        }

        public void CloseDispose()
        {
            Close();
            (_viewModel as IDisposable)?.Dispose();
            _viewModel = null;
        }

        
        private void ToBattle()
        {
            _viewModel.StartMatch();
            Close();
        }
        private void SetupEnemyWidgets(IMatchmakingModel viewModel)
        {
            foreach (var unit in viewModel.GetCharacters())
            {
                UnitWidget unitWidget = Instantiate(_unitWidgetPrefab, _scrollViewRoot);
                unitWidget.SetupWidget(new UnitModel(unit, viewModel.MatchMakingService));
                unitWidget.UnitPortrait.sprite = unit.CharacterIcon;
                
                unitWidget.OnSelected += HandleWidgetSelected;
                
                _enemyWidgets.Add(unitWidget);
            }
        }

        private void HandleWidgetSelected(UnitWidget selectedWidget)
        {
            if (_currentlySelectedWidget == selectedWidget && selectedWidget.IsSelected)
            {
                selectedWidget.SetSelected(false);
                _currentlySelectedWidget = null;
                return;
            }

            if (_currentlySelectedWidget != null)
            {
                _currentlySelectedWidget.SetSelected(false);
            }
            
            _currentlySelectedWidget = selectedWidget;
            _currentlySelectedWidget.SetSelected(true);
        }
        

        private void SetupPlayerWidget(IMatchmakingModel viewModel)
        {
            _playerWidget.UnitPortrait.sprite = viewModel.GetCurrentCharacter()._icon;
        }

        public void ScrollToCurrentOpponent(IMatchmakingModel viewModel)
        {
            //ToDo ScrollToCurrentOpponent
        }

        private void Cleanup()
        {
            foreach (var unit in _enemyWidgets)
            {
                DestroyImmediate(unit.gameObject);
            }  
                    
            _enemyWidgets.Clear();
        }
    }
}