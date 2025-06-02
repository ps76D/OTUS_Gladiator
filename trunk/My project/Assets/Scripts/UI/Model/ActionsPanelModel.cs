using System;
using System.Collections.Generic;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class ActionsPanelModel : IActionsPanelModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        public IReactiveProperty<int> AvailableActionsCount  => _uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions;
        
        public IReactiveProperty<int> MaxActionsCount => _uiManager.ProfileService.PlayerProfile.ActionsService.MaxActionsCount;
        
        public HashSet<ActionModel> AvailableActions {
            get;
            set;
        }

        public HashSet<ActionModel> UnavailableActions {
            get;
            set;
        }

        private readonly List<IDisposable> _disposables = new();
        public ActionsPanelModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            /*_availableActionsCount.Value = uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions.Value;
            _maxActionsCount.Value = uiManager.ProfileService.PlayerProfile.ActionsService.MaxActionsCount;*/
            /*_disposables.Add(AvailableActionsCount.Subscribe(UpdateActionsPanel));
            _disposables.Add(MaxActionsCount.Subscribe(UpdateActionsPanel));*/

            UpdateActions();
            var availableActionsSubscription = uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions.
                Subscribe(UpdateActionsPanel);
            _disposables.Add(availableActionsSubscription);
            
            /*var maxActionsSubscription = uiManager.ProfileService.PlayerProfile.ActionsService.MaxActionsCount.
                Subscribe(UpdateActionsPanel);*/
            

        }
        
        public void SpendAction(int actions)
        {
            _uiManager.ProfileService.PlayerProfile.ActionsService.SpendAction(actions);
        }

        public void UpdateActionsPanel(int actions)
        {
            UpdateActions();
        }

        public void UpdateActions()
        {
            var actionsService = _uiManager.ProfileService.PlayerProfile.ActionsService;

            AvailableActionsCount.Value = actionsService.AvailableActions.Value;
            MaxActionsCount.Value = actionsService.MaxActionsCount.Value;
            
            List<ActionModel> actionsModels = new List<ActionModel>();

            for (int i = 0; i < actionsService.AvailableActions.Value; i++)
            {
                actionsModels.Add(new ActionModel(_uiManager));
            }
            AvailableActions = new HashSet<ActionModel>(actionsModels);
            
            var unavailableActionsCount = actionsService.MaxActionsCount.Value - actionsService.AvailableActions.Value;
            
            List<ActionModel> unavailableActions = new List<ActionModel>();

            for (int i = 0; i < unavailableActionsCount; i++)
            {
                unavailableActions.Add(new ActionModel(_uiManager));
            }
            
            UnavailableActions = new HashSet<ActionModel>(unavailableActions);
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}