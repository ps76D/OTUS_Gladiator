using System.Collections;
using Infrastructure;
using UI.Infrastructure;
using UnityEngine;

namespace UI.Model
{
    public class LosePopupModel : ILosePopupModel
    {
        private readonly UIManager _uiManager;
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
    }
}