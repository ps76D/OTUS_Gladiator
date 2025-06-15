using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UI.Infrastructure;
using UnityEngine;

namespace UI.Model
{
    public class WinPopupModel : IWinPopupModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        private readonly List<IDisposable> _disposables = new();
        
        public WinPopupModel(UIManager uiManager)
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
            _uiManager.HideWinPopup();
            yield return new WaitForSeconds(1f);
            _uiManager.HideBattleScreen();
            /*_uiManager.ShowHud();*/
        }

        public void Dispose()
        {
            Debug.Log("Disposing WinPopupModel");
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}