using System.Collections;
using Infrastructure;
using UI.Infrastructure;
using UnityEngine;

namespace UI.Model
{
    public class WinPopupModel : IWinPopupModel
    {
        private readonly UIManager _uiManager;
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
            _uiManager.HideLosePopup();
            yield return new WaitForSeconds(1f);
            _uiManager.HideBattleScreen();
            /*_uiManager.ShowHud();*/
        }
    }
}