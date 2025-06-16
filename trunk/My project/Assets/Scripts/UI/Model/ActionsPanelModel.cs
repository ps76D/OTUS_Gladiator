using System;
using System.Collections.Generic;
using GameEngine.CharacterSystem;
using UI.Infrastructure;
using UniRx;
using Zenject;

namespace UI.Model
{
    public class ActionsPanelModel : IActionsPanelModel, IDisposable
    {
        private readonly UIManager _uiManager;
        private readonly CharacterService _characterService;
        
        public IReactiveProperty<int> AvailableActionsCount  => _characterService.CurrentCharacterProfile.ActionsService.AvailableActions;
        
        public IReactiveProperty<int> MaxActionsCount => _characterService.CurrentCharacterProfile.ActionsService.MaxActionsCount;
        
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
            _characterService = uiManager.ProfileService.PlayerProfile.CharacterService;
            
            /*_availableActionsCount.Value = uiManager.ProfileService.PlayerProfile.ActionsService.AvailableActions.Value;
            _maxActionsCount.Value = uiManager.ProfileService.PlayerProfile.ActionsService.MaxActionsCount;*/
            /*_disposables.Add(AvailableActionsCount.Subscribe(UpdateActionsPanel));
            _disposables.Add(MaxActionsCount.Subscribe(UpdateActionsPanel));*/

            UpdateActions();
            var maxActionsSubscription = _characterService.CurrentCharacterProfile.ActionsService.MaxActionsCount.
                Subscribe(UpdateActionsPanel);
            var availableActionsSubscription = _characterService.CurrentCharacterProfile.ActionsService.AvailableActions.
                Subscribe(UpdateActionsPanel);
            _disposables.Add(availableActionsSubscription);
            _disposables.Add(maxActionsSubscription);
            
            /*var maxActionsSubscription = uiManager.ProfileService.PlayerProfile.ActionsService.MaxActionsCount.
                Subscribe(UpdateActionsPanel);*/
            

        }
        
        public void SpendAction(int actions)
        {
            _characterService.CurrentCharacterProfile.ActionsService.SpendAction(actions);
        }

        public void UpdateActionsPanel(int actions)
        {
            UpdateActions();
        }

        public void UpdateActions()
        {
            var actionsService = _characterService.CurrentCharacterProfile.ActionsService;

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