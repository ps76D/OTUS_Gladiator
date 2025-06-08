using System;
using System.Collections.Generic;
using GameEngine;
using GameEngine.BattleSystem;
using Infrastructure;
using UI.Infrastructure;
using UniRx;
using Zenject;

namespace UI.Model
{
    public class BattleModel : IBattleModel, IDisposable
    {
        [Inject]
        private BattleService _battleService;
        
        
        
        private readonly UIManager _uiManager;
        /*private readonly CharacterInfo _currentCharacter;
        
        private readonly MatchMakingService _makingService;
        
        [Inject]
        private readonly CharacterDatabase _characterDatabase;
        
        private CharacterInfoSObj _opponentInfoSObj;*/
        
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

            /*_battleService.OnPlayerHealthChanged += UpdatePlayerHealth;
            _battleService.OnPlayerEnergyChanged += UpdatePlayerEnergy;
            _battleService.OnOpponentHealthChanged += UpdateOpponentHealth;
            _battleService.OnOpponentEnergyChanged += UpdateOpponentEnergy;*/
            
            _battleService.OnPlayerWin += PlayerWin;
            _battleService.OnPlayerLose += PlayerLose;
            
            _disposables.Add(_battleService.Player.Health.Subscribe(UpdatePlayerHealth));
            _disposables.Add(_battleService.Player.Energy.Subscribe(UpdatePlayerEnergy));
            _disposables.Add(_battleService.Opponent.Health.Subscribe(UpdateOpponentHealth));
            _disposables.Add(_battleService.Opponent.Energy.Subscribe(UpdateOpponentEnergy));
            
            
            /*_characterDatabase = characterDatabase;
            _currentCharacter = uiManager.ProfileService.PlayerProfile.CharacterService.CurrentCharacterProfile.CharacterInfo;
            _makingService = uiManager.MatchMakingService;*/
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
            _playerHealth.Value = value;
        }
        private void UpdatePlayerEnergy(int value)
        {
            _playerEnergy.Value = value;
        }
        private void UpdateOpponentHealth(int value)
        {
            _opponentHealth.Value = value;
        }
        private void UpdateOpponentEnergy(int value)
        {
            _opponentEnergy.Value = value;
        }

        public void PlayerAttack()
        {
            _battleService.PlayerAttack();
        }
        
        public void PlayerPowerfulAttack()
        {
            if (_battleService.IsPlayerPowerfulAttackPrepared)
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
        
        public void OpponentAttack()
        {
            _battleService.OpponentAttack();
        }
        
        public void OpponentPowerfulAttack()
        {
            if (_battleService.IsOpponentPowerfulAttackPrepared)
            {
                _battleService.OpponentPowerfulAttack();
            }
        }
        
        public void OpponentPrepareAttack()
        { 
            _battleService.OpponentPreparePowerfulAttack();
        }
        
        public void OpponentBlocks()
        {
            _battleService.OpponentBlocks();
        }
        
        public void OpponentSkipTurn()
        {
            _battleService.OpponentSkipTurn();
        }
        
        public void OpponentGiveUp()
        {
            _battleService.OpponentGiveUp();
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        public void PlayerWin()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<WinState>();
        }
        
        public void PlayerLose()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoseState>();
        }
    }
}