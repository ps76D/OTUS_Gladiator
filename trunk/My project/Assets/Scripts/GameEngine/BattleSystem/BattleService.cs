using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.AI;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameEngine
{
    [Serializable]
    public class BattleService : MonoBehaviour, IDisposable
    {
        [Inject]
        [SerializeField] private BattleConfig _battleConfig;
        public BattleConfig BattleConfig  => _battleConfig;
        
        [Inject]
        private BrainStateMachine _brainStateMachine;
        public BrainStateMachine BrainStateMachine => _brainStateMachine;

        [SerializeField] private UnitBattleData _player = new ();
        [SerializeField] private UnitBattleData _opponent = new ();

        private float _playerAnimTime;
        
        [SerializeField] private bool _isPlayerBlocks;
        [SerializeField] private bool _isOpponentBlocks;

        public IReadOnlyReactiveProperty<bool> IsPlayerPowerfulAttackPrepared => _isPlayerPowerfulAttackPrepared;
        private readonly ReactiveProperty<bool> _isPlayerPowerfulAttackPrepared = new ();
        
        [SerializeField] private bool _isOpponentPowerfulAttackPrepared;
        [SerializeField] private bool _isEnoughOpponentEnergy;
        [SerializeField] private bool _isPlayerDead;
        
        public IReadOnlyReactiveProperty<bool> IsPlayerTurn => _isPlayerTurn;
        private readonly ReactiveProperty<bool> _isPlayerTurn = new ();

        public float PlayerAnimTime => _playerAnimTime;

        public bool IsOpponentPowerfulAttackPrepared => _isOpponentPowerfulAttackPrepared;

        public UnitBattleData Player => _player;
        public UnitBattleData Opponent => _opponent;

        public Action<int> OnPlayerTakeDamage;
        public Action<int> OnOpponentTakeDamage;
        
        public Action OnPlayerBlocked;
        public Action OnOpponentBlocked;
        
        public Action OnPlayerDodge;
        public Action OnOpponentDodge;
        
        public Action<int> OnPlayerEnergySpent;
        public Action<int> OnOpponentEnergySpent;

        public Action OnPlayerAttack;
        public Action OnOpponentAttack;

        public Action OnPlayerWin;
        public Action OnPlayerLose;

        public Action OnPlayerDead;
        public Action OnOpponentDead;
        
        public Action OnOpponentTurn;
        public Action OnOpponentTurnEnd;
        
        public Action OnOpponentDecideToPowerfulAttack;
        public Action OnOpponentDecideToAttack;
        public Action OnOpponentDecideToBlock;
        public Action OnOpponentDecideToNotBlock;
        
        private readonly List<IDisposable> _disposables = new();

        public void Init(UnitBattleData player, UnitBattleData opponent)
        {
            _brainStateMachine.Initialize(this);
            _brainStateMachine.Enter<WaitTurnNode>();
     
            _player = player;
            _opponent = opponent;
            
            _isPlayerDead = false;

            _isPlayerBlocks = false; 
            _isOpponentBlocks = false;
            _isPlayerPowerfulAttackPrepared.Value = false; 
            _isOpponentPowerfulAttackPrepared = false;
            _isEnoughOpponentEnergy = false;
            
            _isPlayerTurn.Value = true;
            
            _disposables.Add(_player.Health.Subscribe(CheckPlayerHealthStats));
            _disposables.Add(_player.Energy.Subscribe(CheckPlayerEnergyStats));
            
            _disposables.Add(_opponent.Health.Subscribe(CheckOpponentHealthStats));
            _disposables.Add(_opponent.Energy.Subscribe(CheckOpponentEnergyStats));
            
            _disposables.Add(_isPlayerTurn.Subscribe(x => _isPlayerTurn.Value = CheckIsPlayerTurn()));
        }
        
        public IEnumerator SetIsPlayerTurnCoroutine()
        {
            OnOpponentTurnEnd?.Invoke();
            
            yield return new WaitForSeconds(_battleConfig.NextTurnDelayTime);
            
            _isPlayerBlocks = false;
            _isPlayerTurn.Value = true;
        }
        
        [Button]
        public void SetIsPlayerTurn()
        {
            StartCoroutine(SetIsPlayerTurnCoroutine());
        }

        public IEnumerator SetIsOpponentTurnCoroutine()
        {
            _isPlayerTurn.Value = false;
            
            yield return new WaitForSeconds(_battleConfig.NextTurnDelayTime);
            
            _isOpponentBlocks = false;
            
            OnOpponentTurn?.Invoke();
        }
        
        [Button]
        public void SetIsOpponentTurn()
        {
            StartCoroutine(SetIsOpponentTurnCoroutine());
        }
        
        public bool CheckIsPlayerTurn()
        {
            return _isPlayerTurn.Value;
        }
        
        [Button]
        public void PlayerAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            
            Debug.Log("<color=green>Player Attack</color>");
            
            OnPlayerAttack?.Invoke();

            _playerAnimTime = _battleConfig.AttackAnimTime;
            
            int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);

            if (dodgeThrow >= _opponent.DodgeChanceValue)
            {
                var resultDamage = _player.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                OnOpponentTakeDamage?.Invoke(resultDamage);
                
                _opponent.Health.Value -= resultDamage;

                if (_isOpponentBlocks)
                {
                    _opponent.Energy.Value -= _opponent.BlockEnergyCostValue;
                    OnOpponentEnergySpent?.Invoke(_opponent.BlockEnergyCostValue);
                    OnOpponentBlocked?.Invoke();
                    Debug.Log("<color=magenta>Opponent Blocks Player's Attack</color>");

                    _isOpponentBlocks = false;
                }
            }
            else
            {
                Debug.Log("<color=magenta>Opponent Dodge PlayerAttack</color>");
                OnOpponentDodge?.Invoke();
            }
            _isOpponentBlocks = false;

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentAttack()
        {
            if (_isPlayerDead) return;
            if (CheckIsPlayerTurn()) return;
            
            _playerAnimTime = _battleConfig.AttackAnimTime;
            
            OnOpponentAttack?.Invoke();
            
            int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);

            if (dodgeThrow >= _player.DodgeChanceValue)
            {
                var resultDamage = _opponent.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                OnPlayerTakeDamage?.Invoke(resultDamage);
                
                _player.Health.Value -= resultDamage;
                
                if (_isPlayerBlocks)
                {
                    _player.Energy.Value -= _player.BlockEnergyCostValue;
                    OnPlayerBlocked?.Invoke();
                    OnPlayerEnergySpent?.Invoke(_player.BlockEnergyCostValue);
                    Debug.Log("<color=green>Player Blocks Opponent's Attack</color>");
                    
                    _isPlayerBlocks = false;
                }
            }
            else
            {
                Debug.Log("<color=green>Player Dodge OpponentAttack</color>");
                OnPlayerDodge?.Invoke();
            }
            _isPlayerBlocks = false;
            
            SetIsPlayerTurn();
        }

        [Button]
        public void PlayerBlocks()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = _battleConfig.BlockAnimTime;
            
            _isPlayerBlocks = true;
            
            Debug.Log("<color=green>Player Blocks</color>");

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentBlocks()
        {
            if (_isPlayerDead) return;
            if (CheckIsPlayerTurn()) return;
            
            _playerAnimTime = _battleConfig.BlockAnimTime;
            
            _isOpponentBlocks = true;
            
            Debug.Log("<color=magenta>Opponent Blocks</color>");
            
            SetIsPlayerTurn();
        }

        [Button]
        public void PlayerPowerfulAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            
            Debug.Log("<color=green>Player PowerfulAttack</color>");
            
            if (_isPlayerPowerfulAttackPrepared.Value)
            {
                OnPlayerAttack?.Invoke();
                
                _playerAnimTime = _battleConfig.PowerfulAttackAnimTime;
                
                int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
                var dodgeThrow = Random.Range(100, 0);

                if (dodgeThrow >= _opponent.DodgeChanceValue)
                {
                    var resultDamage = _player.PowerfulDamageValue - blockValue;

                    if (resultDamage < 0)
                    {
                        resultDamage = 0;
                    }
                    
                    OnOpponentTakeDamage?.Invoke(resultDamage);
                    
                    _opponent.Health.Value -= resultDamage;
                    
                    if (_isOpponentBlocks)
                    {
                        _opponent.Energy.Value -= _opponent.BlockEnergyCostValue;
                        OnOpponentEnergySpent?.Invoke(_opponent.BlockEnergyCostValue);
                        OnOpponentBlocked?.Invoke();
                        Debug.Log("<color=magenta>Opponent Blocks Player's Powerful Attack</color>");
                        
                        _isOpponentBlocks = false;
                    }
                }
                else
                {
                    OnOpponentDodge?.Invoke();
                    Debug.Log("<color=magenta>Opponent Dodge PlayerPowerfulAttack</color>");
                }
                _player.Energy.Value -= _player.PowerfulDamageEnergyCost;
                OnPlayerEnergySpent?.Invoke(_player.PowerfulDamageEnergyCost);
                                
                _isOpponentBlocks = false;
                
                _isPlayerPowerfulAttackPrepared.Value = false;
            }
            else
            {
                Debug.Log("<color=green>Player Powerful Attack Not Prepared</color>");
            }

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentPowerfulAttack()
        {
            if (_isPlayerDead) return;
            if (CheckIsPlayerTurn()) return;
            
            Debug.Log("<color=magenta>Opponent PowerfulAttack</color>");

            OnOpponentAttack?.Invoke();

            _playerAnimTime = _battleConfig.PowerfulAttackAnimTime;

            int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;

            var dodgeThrow = Random.Range(100, 0);

            if (dodgeThrow >= _player.DodgeChanceValue)
            {
                var resultDamage = _opponent.PowerfulDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }

                OnPlayerTakeDamage?.Invoke(resultDamage);

                _player.Health.Value -= resultDamage;
                
                if (_isPlayerBlocks)
                {
                    _player.Energy.Value -= _player.BlockEnergyCostValue;
                    OnPlayerBlocked?.Invoke();
                    OnPlayerEnergySpent?.Invoke(_player.BlockEnergyCostValue);

                    Debug.Log("<color=green>Player Blocks Opponent's Powerful Attack</color>");

                    _isPlayerBlocks = false;
                }
            }
            else
            {
                OnPlayerDodge?.Invoke();
                Debug.Log("<color=green>Player Dodge OpponentPowerfulAttack</color>");
            }

            _opponent.Energy.Value -= _opponent.PowerfulDamageEnergyCost;
            OnOpponentEnergySpent?.Invoke(_opponent.PowerfulDamageEnergyCost);

            _isPlayerBlocks = false;

            _isOpponentPowerfulAttackPrepared = false;

            SetIsPlayerTurn();
        }
        
        [Button]
        public void GiveUp()
        {
            if (!CheckIsPlayerTurn()) return;
            _player.Health.Value = 0;
        }
        
        [Button]
        public void OpponentGiveUp()
        {
            if (CheckIsPlayerTurn()) return;
            _opponent.Health.Value = 0;
        }
        
        [Button]
        public void PlayerSkipTurn()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = _battleConfig.SkipTurnAnimTime;
            
            Debug.Log("<color=green>Player skip turn</color>");

            SetIsOpponentTurn();
        }
        
        [Button]
        public void PlayerPreparePowerfulAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = _battleConfig.PreparePowerfulAttackAnimTime;
            
            _isPlayerPowerfulAttackPrepared.Value = true;

            Debug.Log("<color=green>Player Prepare Powerful Attack</color>");

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentPreparePowerfulAttack()
        {
            if (_isPlayerDead) return;
            if (CheckIsPlayerTurn()) return;
            
            _isOpponentPowerfulAttackPrepared = true;

            Debug.Log("<color=magenta>Opponent Prepare Powerful Attack</color>");
            
            SetIsPlayerTurn();
        }
        
        public void CheckPlayerHealthStats(int value)
        {
            if (_player.Health.Value <= 0)
            {
                OnPlayerDead?.Invoke();
                _isPlayerDead = true;
                StartCoroutine(Lose());
            }
        }

        public void CheckPlayerEnergyStats(int value)
        {
            if (_player.Energy.Value <= 0)
            {
                OnPlayerDead?.Invoke();
                _isPlayerDead = true;
                StartCoroutine(Lose());
            }
        }
        
        public void CheckOpponentHealthStats(int value)
        {
            if (_opponent.Health.Value <= 0)
            {
                OnOpponentDead?.Invoke();

                if (!_isPlayerDead) StartCoroutine(Win());
            }
        }
        
        public void CheckOpponentEnergyStats(int value)
        {
            if (_opponent.Energy.Value <= 0)
            {
                OnOpponentDead?.Invoke();
                
                if (!_isPlayerDead) StartCoroutine(Win());
            }
        }
        
        [Button]
        public IEnumerator Win()
        {
            yield return new WaitForSecondsRealtime(_battleConfig.WinPopupDelayTime);
            
            OnPlayerWin?.Invoke();
            Debug.Log("<color=green>Player Win</color>");
        }
        
        [Button]
        public IEnumerator Lose()
        {
            yield return new WaitForSecondsRealtime(_battleConfig.LosePopupDelayTime);
            
            OnPlayerLose?.Invoke();
            Debug.Log("<color=green>Player Lose</color>");
        }
        
        public void ThrowDoBlocking(int value)
        {
            var chance = Random.Range(0, 100);
            if (chance >= value )
            {
                Debug.Log("<color=magenta>Opponent Decided to Block</color>");
                OnOpponentDecideToBlock?.Invoke();
            }
            else
            {
                OnOpponentDecideToNotBlock?.Invoke();
                Debug.Log("<color=magenta>Opponent Decided to Choose Attack</color>");
            }
        }

        public void ThrowChooseAttackType(int value)
        {
            var chance = Random.Range(0, 100);
            if (chance >= value)
            {
                Debug.Log("<color=magenta>Opponent Decided to Prepare Powerful Attack</color>");
                OnOpponentDecideToPowerfulAttack?.Invoke();
            }
            else
            {
                Debug.Log("<color=magenta>Opponent Decided to Attack</color>");
                OnOpponentDecideToAttack?.Invoke();
            }
        }

        public bool CheckOpponentEnergyIsEnough()
        {
            bool value = false;
            
            Debug.Log("<color=magenta>Check Opponent Energy If Enough For Powerful Attack : </color>" + _opponent.Energy.Value);
            if (_opponent.Energy.Value >= _opponent.PowerfulDamageEnergyCost)
            {
                value = true;
            }
            return value;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}