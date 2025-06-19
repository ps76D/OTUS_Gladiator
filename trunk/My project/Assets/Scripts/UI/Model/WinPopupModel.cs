using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UI.Infrastructure;
using UniRx;
using UnityEngine;

namespace UI.Model
{
    public class WinPopupModel : IWinPopupModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        private readonly List<IDisposable> _disposables = new();
        public IReadOnlyReactiveProperty<int> RewardCount => _rewardCount;
        private readonly ReactiveProperty<int> _rewardCount = new(0);

        public IReadOnlyReactiveProperty<string> MoralLevelChanged => _moralLevelChanged;
        private readonly ReactiveProperty<string> _moralLevelChanged = new();
        
        
        public WinPopupModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            _disposables.Add(_uiManager.ProfileService.MoralService.CurrentMoral.Subscribe(ChangeMoralText));
            
            GiveRewards();
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
            _uiManager.MatchmakingView.CloseDispose();
        }

        public void GiveRewards()
        {
            GiveMoneyReward(_uiManager.MatchMakingService.CurrentOpponent.RewardForDefeatEnemy);
            ChangeMoral();
        }

        public void ChangeMoral()
        {
            _uiManager.ProfileService.MoralService.IncreaseMoral(20);
        }

        public void GiveMoneyReward(int reward)
        {
            _uiManager.ProfileService.MoneyStorage.AddMoney(reward);
            _rewardCount.Value = reward;
        }

        public void ChangeMoralText(int value)
        {
            _moralLevelChanged.Value = _uiManager.ProfileService.MoralService.GetMoralLevel().MoralLevelText.GetLocalizedString();
        }

        public void Dispose()
        {
            Debug.Log("Disposing WinPopupModel");
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}