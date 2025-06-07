using System;
using System.Collections.Generic;
using GameEngine.CharacterSystem;
using UI.Model;
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
        }

        private void OnDisable()
        {
            Cleanup();
            
            _backButton.onClick.RemoveListener(Close);
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
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
                
                _enemyWidgets.Add(unitWidget);
            }
        }

        private void SetupPlayerWidget(IMatchmakingModel viewModel)
        {
            _playerWidget.UnitPortrait.sprite = viewModel.GetCurrentCharacter()._icon;
        }

        public void ScrollToCurrentOpponent(IMatchmakingModel viewModel)
        {
            
        }

        private void Cleanup()
        {
            /*if (_enemyWidgets == null) return;
            if (_enemyWidgets.Count == 0) return;*/
            foreach (var unit in _enemyWidgets)
            {
                DestroyImmediate(unit.gameObject);
            }  
                    
            _enemyWidgets.Clear();
        }
    }
}