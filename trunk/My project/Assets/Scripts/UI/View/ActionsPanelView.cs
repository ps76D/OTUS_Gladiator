using System;
using System.Collections.Generic;
using UI.Model;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ActionsPanelView : MonoBehaviour, IDisposable
    {
        [SerializeField] private ActionView _availableActionPrefab;
        [SerializeField] private ActionView _unavailableActionPrefab;
        
        private readonly List<ActionView> _actionViews = new();
        
        private readonly List<IDisposable> _disposables = new();
        
        private IActionsPanelModel _viewModel;
        
        public void Show(IActionsPanelModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _disposables.Add(viewModel.AvailableActionsCount.Subscribe(UpdateActionsPanel));
            _disposables.Add(viewModel.MaxActionsCount.Subscribe(UpdateActionsPanel));
            
            Cleanup();
            UpdateActions(_viewModel);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Cleanup();
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        private void Cleanup()
        {
            if (_actionViews == null) return;
            foreach (var actionView in _actionViews)
            {
                DestroyImmediate(actionView.gameObject);
            }
            _actionViews.Clear();
        }

        public void UpdateActionsPanel(int actionsCount)
        {
            Cleanup();
            UpdateActions(_viewModel);
        }
        
        private void UpdateActions(IActionsPanelModel viewModel)
        {
            foreach (var statModel in viewModel.AvailableActions)
            {
                var statWidget = Instantiate(_availableActionPrefab, gameObject.transform);
                statWidget.Show(statModel);
                _actionViews.Add(statWidget);
            }

            if (viewModel.UnavailableActions.Count > 0)
            {
                foreach (var statModel in viewModel.UnavailableActions)
                {
                    var statWidget = Instantiate(_unavailableActionPrefab, gameObject.transform);
                    statWidget.Show(statModel);
                    _actionViews.Add(statWidget);
                }
            }
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}