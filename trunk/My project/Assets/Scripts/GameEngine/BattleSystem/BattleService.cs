using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.BattleSystem;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameEngine
{
    [Serializable]
    public class BattleService : MonoBehaviour, IDisposable
    {
        [SerializeField] private BattleSceneView _battleSceneView;
        
        [SerializeField] private UnitBattleData _player = new ();
        [SerializeField] private UnitBattleData _opponent = new ();

        [SerializeField] private bool _isPlayerBlocks;
        [SerializeField] private bool _isOpponentBlocks;
        
        [SerializeField] private bool _isPlayerPowerfulAttackPrepared;
        [SerializeField] private bool _isOpponentPowerfulAttackPrepared;

        private float _playerAnimTime; 

        public IReadOnlyReactiveProperty<bool> IsPlayerTurn => _isPlayerTurn;
        private readonly ReactiveProperty<bool> _isPlayerTurn = new ();
        
        public IReadOnlyReactiveProperty<bool> IsOpponentTurn => _isOpponentTurn;
        private readonly ReactiveProperty<bool> _isOpponentTurn = new ();
        
        private readonly List<IDisposable> _disposables = new();
        
        public bool IsPlayerBlocks => _isPlayerBlocks;
        public bool IsOpponentBlocks => _isOpponentBlocks;
        public bool IsPlayerPowerfulAttackPrepared => _isPlayerPowerfulAttackPrepared;
        public bool IsOpponentPowerfulAttackPrepared => _isOpponentPowerfulAttackPrepared;
        
        public UnitBattleData Player => _player;
        public UnitBattleData Opponent => _opponent;

        public Action<int> OnPlayerHealthChanged;
        public Action<int> OnOpponentHealthChanged;
        public Action<int> OnPlayerEnergyChanged;
        public Action<int> OnOpponentEnergyChanged;
        
        public Action<int> OnPlayerTakeDamage;
        public Action<int> OnOpponentTakeDamage;
        public Action OnPlayerBlocked;
        public Action OnOpponentBlocked;
        public Action OnPlayerDodge;
        public Action OnOpponentDodge;
        public Action<int> OnPlayerEnduranceSpent;
        public Action<int> OnOpponentEnduranceSpent;
        
        public Action OnPlayerAttack;
        public Action OnOpponentAttack;
        
        public Action OnPlayerWin;
        public Action OnPlayerLose;
        
        public Action OnPlayerDead;
        public Action OnOpponentDead;

        public void Init(UnitBattleData player, UnitBattleData opponent)
        {
            _player = player;
            _opponent = opponent;

            _disposables.Add(_player.Health.Subscribe(CheckPlayerHealthStats));
            _disposables.Add(_player.Energy.Subscribe(CheckPlayerEnergyStats));
            
            _disposables.Add(_opponent.Health.Subscribe(CheckOpponentHealthStats));
            _disposables.Add(_opponent.Energy.Subscribe(CheckOpponentEnergyStats));
            
            SetIsPlayerTurn();
            _disposables.Add(_isPlayerTurn.Subscribe(x => _isPlayerTurn.Value = CheckIsPlayerTurn()));
            _disposables.Add(_isOpponentTurn.Subscribe(x => _isOpponentTurn.Value = OpponentDecision()));

        }
        
        [Button]
        public void SetIsPlayerTurn()
        {
            _isPlayerTurn.Value = true;
            _isOpponentTurn.Value = false;
        }
        [Button]
        public void SetIsOpponentTurn()
        {
            _isPlayerTurn.Value = false;
            _isOpponentTurn.Value = true;
        }
        
        public bool CheckIsPlayerTurn()
        {
            return _isPlayerTurn.Value;
        }
        
        [Button]
        public void PlayerAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            
            OnPlayerAttack?.Invoke();

            _playerAnimTime = 1.5f;
            
            int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);
            /*Debug.Log(dodgeThrow);*/
            if (dodgeThrow >= _opponent.DodgeChanceValue)
            {
                var resultDamage = _player.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                OnOpponentTakeDamage?.Invoke(resultDamage);
                
                _opponent.Health.Value -= resultDamage;
                OnOpponentHealthChanged?.Invoke(_opponent.Health.Value);

                if (_isOpponentBlocks)
                {
                    OnOpponentBlocked?.Invoke();
                }
            }
            else
            {
                Debug.Log("Dodge");
                OnOpponentDodge?.Invoke();
            }
            _isOpponentBlocks = false;

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentAttack()
        {
            if (CheckIsPlayerTurn()) return;
            
            OnOpponentAttack?.Invoke();
            
            int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);
            /*Debug.Log(dodgeThrow);*/
            if (dodgeThrow >= _player.DodgeChanceValue)
            {
                var resultDamage = _opponent.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                OnPlayerTakeDamage?.Invoke(resultDamage);
                
                _player.Health.Value -= resultDamage;
                OnPlayerHealthChanged?.Invoke(_player.Health.Value);
                
                if (_isPlayerBlocks)
                {
                    OnPlayerBlocked?.Invoke();
                }
            }
            else
            {
                Debug.Log("Dodge");
                OnPlayerDodge?.Invoke();
            }
            _isPlayerBlocks = false;
            
            SetIsPlayerTurn();
        }

        [Button]
        public void PlayerBlocks()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = 0.5f;
            
            _isPlayerBlocks = true;
            _player.Energy.Value -= _player.BlockEnduranceCostValue;
            OnPlayerEnergyChanged?.Invoke(_player.Energy.Value);
            
            Debug.Log("Player Blocks");
            PlayerSkipTurn();

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentBlocks()
        {
            if (CheckIsPlayerTurn()) return;
            _isOpponentBlocks = true;
            _opponent.Energy.Value -= _opponent.BlockEnduranceCostValue;
            OnOpponentEnergyChanged?.Invoke(_opponent.Energy.Value);
            
            Debug.Log("Opponent Blocks");
            OpponentSkipTurn();
            
            SetIsPlayerTurn();
        }

        [Button]
        public void PlayerPowerfulAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            if (_isPlayerPowerfulAttackPrepared)
            {
                OnPlayerAttack?.Invoke();
                
                _playerAnimTime = 1.5f;
                
                int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
                var dodgeThrow = Random.Range(100, 0);
                /*Debug.Log(dodgeThrow);*/
                if (dodgeThrow >= _opponent.DodgeChanceValue)
                {
                    var resultDamage = _player.PowerfulDamageValue - blockValue;

                    if (resultDamage < 0)
                    {
                        resultDamage = 0;
                    }
                    
                    OnOpponentTakeDamage?.Invoke(resultDamage);
                    
                    _opponent.Health.Value -= resultDamage;
                    OnOpponentHealthChanged?.Invoke(_opponent.Health.Value);
                    
                    if (_isOpponentBlocks)
                    {
                        OnOpponentBlocked?.Invoke();
                        OnOpponentEnduranceSpent?.Invoke(_opponent.BlockEnduranceCostValue);
                    }
                    
                    _player.Energy.Value -= _player.PowerfulDamageEnduranceCost;
                    OnPlayerEnduranceSpent?.Invoke(_player.PowerfulDamageEnduranceCost);
                    OnPlayerEnergyChanged?.Invoke(_player.Energy.Value);
                }
                else
                {
                    OnOpponentDodge?.Invoke();
                    Debug.Log("Dodge");
                    OnPlayerEnduranceSpent?.Invoke(_player.PowerfulDamageEnduranceCost);
                    OnPlayerEnergyChanged?.Invoke(_player.Energy.Value);
                }
                _isOpponentBlocks = false;
                
                _isPlayerPowerfulAttackPrepared = false;
            }
            else
            {
                Debug.Log("Player Powerful Attack Not Prepared ");
            }

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentPowerfulAttack()
        {
            if (CheckIsPlayerTurn()) return;
            if (_isOpponentPowerfulAttackPrepared)
            {
                OnOpponentAttack?.Invoke();
                
                int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;
            
                var dodgeThrow = Random.Range(100, 0);
                /*Debug.Log(dodgeThrow);*/
                if (dodgeThrow >= _player.DodgeChanceValue)
                {
                    var resultDamage = _opponent.PowerfulDamageValue - blockValue;

                    if (resultDamage < 0)
                    {
                        resultDamage = 0;
                    }
                    
                    OnPlayerTakeDamage?.Invoke(resultDamage);
                    
                    _player.Health.Value -= resultDamage;
                    OnPlayerHealthChanged?.Invoke(_player.Health.Value);
                    
                    if (_isPlayerBlocks)
                    {
                        OnPlayerBlocked?.Invoke();
                        OnPlayerEnduranceSpent?.Invoke(_player.BlockEnduranceCostValue);
                    }
                    
                    _opponent.Energy.Value -= _opponent.PowerfulDamageEnduranceCost;
                    OnOpponentEnduranceSpent?.Invoke(_opponent.PowerfulDamageEnduranceCost);
                    OnOpponentEnergyChanged?.Invoke(_opponent.Energy.Value);
                }
                else
                {
                    OnPlayerDodge?.Invoke();
                    Debug.Log("Dodge");
                    OnOpponentEnduranceSpent?.Invoke(_opponent.PowerfulDamageEnduranceCost);
                    OnOpponentEnergyChanged?.Invoke(_opponent.Energy.Value);
                }
                _isPlayerBlocks = false;
                
                _isOpponentPowerfulAttackPrepared = false;
            }
            else
            {
                Debug.Log("Opponent Powerful Attack Not Prepared ");
            }
            
            SetIsPlayerTurn();
        }
        
        [Button]
        public void GiveUp()
        {
            if (!CheckIsPlayerTurn()) return;
            _player.Health.Value = 0;
            OnPlayerHealthChanged?.Invoke(_player.Health.Value);
        }
        
        [Button]
        public void OpponentGiveUp()
        {
            if (CheckIsPlayerTurn()) return;
            _opponent.Health.Value = 0;
            OnOpponentHealthChanged?.Invoke(_opponent.Health.Value);
            
            SetIsPlayerTurn();
        }
        
        [Button]
        public void PlayerSkipTurn()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = 1f;
            
            Debug.Log("Player skip turn");

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentSkipTurn()
        {
            if (CheckIsPlayerTurn()) return;
            Debug.Log("Opponent skip turn");
            
            SetIsPlayerTurn();
        }
        
        [Button]
        public void PlayerPreparePowerfulAttack()
        {
            if (!CheckIsPlayerTurn()) return;
            
            _playerAnimTime = 1f;
            
            _isPlayerPowerfulAttackPrepared = true;
            PlayerSkipTurn();
            Debug.Log("Player Prepare Powerful Attack");

            SetIsOpponentTurn();
        }
        
        [Button]
        public void OpponentPreparePowerfulAttack()
        {
            if (CheckIsPlayerTurn()) return;
            _isOpponentPowerfulAttackPrepared = true;
            OpponentSkipTurn();
            Debug.Log("Opponent Prepare Powerful Attack");
            
            SetIsPlayerTurn();
        }
        
        public void CheckPlayerHealthStats(int value)
        {
            Debug.Log("Check Player Health Stats: " + _player.Health.Value);
            if (_player.Health.Value <= 0)
            {
                OnPlayerDead?.Invoke();
                StartCoroutine(Lose());
            }
        }

        public void CheckPlayerEnergyStats(int value)
        {
            Debug.Log("Check Player Energy Stats: " + _player.Energy.Value);
            if (_player.Energy.Value <= 0)
            {
                OnPlayerDead?.Invoke();
                StartCoroutine(Lose());
            }
        }
        
        public void CheckOpponentHealthStats(int value)
        {
            Debug.Log("Check Opponent Health Stats: " + _opponent.Health.Value);
            if (_opponent.Health.Value <= 0)
            {
                OnOpponentDead?.Invoke();
                StartCoroutine(Win());
            }
        }
        
        public void CheckOpponentEnergyStats(int value)
        {
            Debug.Log("Check Opponent Energy Stats: " + _opponent.Energy.Value);
            if (_opponent.Energy.Value <= 0)
            {
                OnOpponentDead?.Invoke();
                StartCoroutine(Win());
            }
        }
        
        [Button]
        public IEnumerator Win()
        {
            yield return new WaitForSecondsRealtime(2f);
            
            OnPlayerWin?.Invoke();
            Debug.Log("Player Win");
        }
        
        [Button]
        public IEnumerator Lose()
        {
            yield return new WaitForSecondsRealtime(2f);
            
            OnPlayerLose?.Invoke();
            Debug.Log("Player Lose");
        }

        
        
        public IEnumerator OpponentDecisionCoroutine()
        {
            yield return new WaitForSecondsRealtime(_playerAnimTime);

            DoOpponentDecision();
        }
        
        public bool OpponentDecision()
        {
            StartCoroutine(OpponentDecisionCoroutine());
            
            return _isOpponentTurn.Value;
        }
        
        public void DoOpponentDecision()
        {
            if (_isOpponentTurn.Value)
            {
                if (CheckIfNeedToPowerfulAttack())
                {
                    Debug.Log("Opponent Finish Prepared Attack ");
                }
                else
                {
                    if (_isPlayerPowerfulAttackPrepared)
                    {
                        ThrowDoBlocking(50);
                        
                    }
                    else
                    {
                        ThrowChooseAttackType(70);
                    }
                }
                

                
                Debug.Log("Opponent Decision Final");
            }
        }

        private void ThrowDoBlocking(int value)
        {
            var chance = Random.Range(0, 100);
            if (chance >= value )
            {
                Debug.Log("Opponent Decided to Block");
                OpponentBlocks();
            }
            else
            {
                Debug.Log("Opponent Decided to Choose Attack");
                ThrowChooseAttackType(70);
            }
            
        }

        private void ThrowChooseAttackType(int value)
        {
            var chance = Random.Range(0, 100);
            if (chance >= value)
            {
                Debug.Log("Opponent Decided to Prepare Powerful Attack");
                OpponentPreparePowerfulAttack();
            }
            else
            {
                Debug.Log("Opponent Decided to Attack");
                OpponentAttack();
            }
        }

        private bool CheckIfNeedToPowerfulAttack()
        {
            var currentValue = _isOpponentPowerfulAttackPrepared;
            
            if (_isOpponentPowerfulAttackPrepared)
            {
                OpponentPowerfulAttack();
            }

            return currentValue;
        }


        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}