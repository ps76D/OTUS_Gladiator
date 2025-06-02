using System.Collections.Generic;
using UI.Model;
using UnityEngine;

namespace UI
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField] private StatView _statViewWidget;
        private IStatsModel _viewModel;
        /*public IStatsModel ViewModel => _viewModel;*/
        
        /*private readonly List<IDisposable> _disposables = new();*/
        
        private readonly List<StatView> _statViews = new();
        /*public List<StatView> StatViews => _statViews;*/
        
        public void Show(IStatsModel viewModel)
        {
            _viewModel = viewModel;
            
            gameObject.SetActive(true);

            Cleanup();
            
            UpdateStats(_viewModel);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            Cleanup();
            
            /*foreach (var disposable in _disposables)
                disposable.Dispose();*/
        }

        private void UpdateStats(IStatsModel viewModel)
        {
            foreach (var statModel in viewModel.Stats)
            {
                var statWidget = Instantiate(_statViewWidget, gameObject.transform);
                statWidget.Show(statModel);
                _statViews.Add(statWidget);
            }
        }

        private void Cleanup()
        {
            if (_statViews == null) return;
            foreach (var statView in _statViews)
            {
                DestroyImmediate(statView.gameObject);
            }
            _statViews.Clear();
        }
    }
}