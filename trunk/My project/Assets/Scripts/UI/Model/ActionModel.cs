using System;
using System.Collections.Generic;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class ActionModel : IActionModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        public IReadOnlyReactiveProperty<bool> ActionAvailable => _actionAvailable;
        private readonly ReactiveProperty<bool> _actionAvailable = new ();
        
        private readonly List<IDisposable> _disposables = new();
        
        public ActionModel(UIManager uiManager)
        {
            _uiManager = uiManager;

            /*
            var actionAvailableSubscription = _uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions.
                Subscribe(CheckIsActionAvailable);
            _disposables.Add(levelUpInteractableSubscription);
            */

        }

        private bool CheckIsActionAvailable()
        {
            return _actionAvailable.Value;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}