using System;
using System.Collections.Generic;
using GameEngine;
using Infrastructure;
using UI.Infrastructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Model
{
    public class BattleModel : IBattleModel, IDisposable
    {
        [Inject]
        private BattleService _battleService;
        public BattleService BattleService => _battleService;
        
        private readonly UIManager _uiManager;
        
        public IReadOnlyReactiveProperty<int> PlayerHealth => _playerHealth;
        private readonly ReactiveProperty<int> _playerHealth = new(0);
        public IReadOnlyReactiveProperty<int> PlayerEnergy => _playerEnergy;
        private readonly ReactiveProperty<int> _playerEnergy = new(0);
        
        public IReadOnlyReactiveProperty<int> OpponentHealth => _opponentHealth;
        private readonly ReactiveProperty<int> _opponentHealth = new(0);
        public IReadOnlyReactiveProperty<int> OpponentEnergy => _opponentEnergy;
        private readonly ReactiveProperty<int> _opponentEnergy = new(0);
        
        private readonly List<IDisposable> _disposables = new();
        
        public BattleModel(UIManager uiManager, BattleService battleService)
        {
            _uiManager = uiManager;
            _battleService = battleService;
            
            _battleService.OnPlayerWin += PlayerWin;
            _battleService.OnPlayerLose += PlayerLose;
            
            _disposables.Add(_battleService.Player.Health.Subscribe(UpdatePlayerHealth));
            _disposables.Add(_battleService.Player.Energy.Subscribe(UpdatePlayerEnergy));
            _disposables.Add(_battleService.Opponent.Health.Subscribe(UpdateOpponentHealth));
            _disposables.Add(_battleService.Opponent.Energy.Subscribe(UpdateOpponentEnergy));
        }

        public int GetPlayerFullHealth()
        {
            return _battleService.Player.FullHealth;
        }
        
        public int GetPlayerFullEnergy()
        {
            return _battleService.Player.FullEnergy;
        }
        
        public int GetOpponentFullHealth()
        {
            return _battleService.Opponent.FullHealth;
        }
        
        public int GetOpponentFullEnergy()
        {
            return _battleService.Opponent.FullEnergy;
        }
        
        private void UpdatePlayerHealth(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            
            _playerHealth.Value = value;
        }
        
        private void UpdatePlayerEnergy(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            
            _playerEnergy.Value = value;
        }
        
        private void UpdateOpponentHealth(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            
            _opponentHealth.Value = value;
        }
        
        private void UpdateOpponentEnergy(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            
            _opponentEnergy.Value = value;
        }

        public void PlayerAttack()
        {
            _battleService.PlayerAttack();
        }
        
        public void PlayerPowerfulAttack()
        {
            if (_battleService.IsPlayerPowerfulAttackPrepared.Value)
            {
                _battleService.PlayerPowerfulAttack();
            }
        }
        
        public void PlayerPrepareAttack()
        {
            _battleService.PlayerPreparePowerfulAttack();
        }
        
        public void PlayerBlocks()
        {
            _battleService.PlayerBlocks();
        }
        
        public void PlayerSkipTurn()
        {
            _battleService.PlayerSkipTurn();
        }
        
        public void PlayerGiveUp()
        {
            _battleService.GiveUp();
        }

        private void PlayerWin()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<WinState>();
        }

        private void PlayerLose()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoseState>();
        }

        public void Dispose()
        {
            Debug.Log("Disposing BattleModel");
            
            _battleService.OnPlayerWin -= PlayerWin;
            _battleService.OnPlayerLose -= PlayerLose;
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}