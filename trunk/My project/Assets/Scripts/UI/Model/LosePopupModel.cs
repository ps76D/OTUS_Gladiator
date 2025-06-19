using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UI.Infrastructure;
using UniRx;
using UnityEngine;

namespace UI.Model
{
    public class LosePopupModel : ILosePopupModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        public IReadOnlyReactiveProperty<string> MoralLevelChanged => _moralLevelChanged;
        private readonly ReactiveProperty<string> _moralLevelChanged = new();
        
        private readonly List<IDisposable> _disposables = new();
        public LosePopupModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            _disposables.Add(_uiManager.ProfileService.MoralService.CurrentMoral.Subscribe(ChangeMoralText));
            
            ChangeMoral();
        }

        public void BackToTraining()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
            
            _uiManager.StartCoroutine(SwitchToTrainingScreen());
        }
        
        private IEnumerator SwitchToTrainingScreen()
        {
            _uiManager.HideLosePopup();
            yield return new WaitForSeconds(1f);
            _uiManager.HideBattleScreen();
        }
        
        public void ChangeMoral()
        {
            _uiManager.ProfileService.MoralService.DegreaseMoral(20);
        }
        
        public void ChangeMoralText(int value)
        {
            _moralLevelChanged.Value = _uiManager.ProfileService.MoralService.GetMoralLevel().MoralLevelText.GetLocalizedString();
        }
        
        public void Dispose()
        {
            Debug.Log("Disposing LosePopupModel");
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}