using System;
using System.Collections;
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
        
        /*public Action OnBattleButtonClicked;*/

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
        
        public void Close()
        {            
            Cleanup();
            
            _backButton.onClick.RemoveListener(Close);
            _fadeCloseButton.onClick.RemoveListener(Close);
            _toBattleButton.onClick.RemoveListener(ToBattle);

            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            /*_viewModel.Dispose();*/
            
            gameObject.SetActive(false);
        }

        public void CloseDispose()
        {
            Close();
            (_viewModel as IDisposable)?.Dispose(); // Уничтожаем модель
            _viewModel = null;
        }

        /*private IEnumerator DisposeCoroutine(IDisposable viewModel)
        {
            yield return new WaitForSeconds(1f);
            yield return null;

            viewModel.Dispose(); // Уничтожаем модель
        }*/
        
        private void ToBattle()
        {
            _viewModel.StartMatch();
            /*OnBattleButtonClicked?.Invoke();*/
            Close();
            /*_uiManager.HideMatchmakingScreen();*/

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
            foreach (var unit in _enemyWidgets)
            {
                DestroyImmediate(unit.gameObject);
            }  
                    
            _enemyWidgets.Clear();
        }
    }
}