using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UI.Infrastructure;
using UnityEngine;

namespace UI.Model
{
    public class LosePopupModel : ILosePopupModel, IDisposable
    {
        private readonly UIManager _uiManager;
        private readonly List<IDisposable> _disposables = new();
        public LosePopupModel(UIManager uiManager)
        {
            _uiManager = uiManager;
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
            /*_uiManager.ShowHud();*/
        }
        
        public void Dispose()
        {
            Debug.Log("Disposing LosePopupModel");
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}