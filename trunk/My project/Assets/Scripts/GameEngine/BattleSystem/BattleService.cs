using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameEngine.BattleSystem
{
    [Serializable]
    public class BattleService : MonoBehaviour, IDisposable
    {
        [SerializeField] private UnitBattleData _player = new ();
        [SerializeField] private UnitBattleData _opponent = new ();

        [SerializeField] private bool _isPlayerBlocks;
        [SerializeField] private bool _isOpponentBlocks;
        
        [SerializeField] private bool _isPlayerPowerfulAttackPrepared;
        [SerializeField] private bool _isOpponentPowerfulAttackPrepared;
        
        private readonly List<IDisposable> _disposables = new();

        public void Init(UnitBattleData player, UnitBattleData opponent)
        {
            _player = player;
            _opponent = opponent;

            _disposables.Add(_player.Health.Subscribe(CheckPlayerHealthStats));
            _disposables.Add(_player.Endurance.Subscribe(CheckPlayerEnduranceStats));
            
            _disposables.Add(_opponent.Health.Subscribe(CheckOpponentHealthStats));
            _disposables.Add(_opponent.Endurance.Subscribe(CheckOpponentEnduranceStats));
        }
        
        [Button]
        public void PlayerAttack()
        {
            int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);
            Debug.Log(dodgeThrow);
            if (dodgeThrow >= _opponent.DodgeChanceValue)
            {
                var resultDamage = _player.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                _opponent.Health.Value -= resultDamage;
            }
            else
            {
                Debug.Log("Dodge");
            }
            _isOpponentBlocks = false;
        }
        
        [Button]
        public void OpponentAttack()
        {
            int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;
            
            var dodgeThrow = Random.Range(100, 0);
            Debug.Log(dodgeThrow);
            if (dodgeThrow >= _player.DodgeChanceValue)
            {
                var resultDamage = _opponent.BaseDamageValue - blockValue;

                if (resultDamage < 0)
                {
                    resultDamage = 0;
                }
                
                _player.Health.Value -= resultDamage;
            }
            else
            {
                Debug.Log("Dodge");
            }
            _isPlayerBlocks = false;
        }

        [Button]
        public void PlayerBlocks()
        {
            _isPlayerBlocks = true;
            _player.Endurance.Value -= _player.BlockEnduranceCostValue;
            Debug.Log("Player Blocks");
            PlayerSkipTurn();
        }
        
        [Button]
        public void OpponentBlocks()
        {
            _isOpponentBlocks = true;
            _opponent.Endurance.Value -= _opponent.BlockEnduranceCostValue;
            Debug.Log("Opponent Blocks");
            OpponentSkipTurn();
        }

        [Button]
        public void PlayerPowerfulAttack()
        {
            if (_isPlayerPowerfulAttackPrepared)
            {
                int blockValue = _isOpponentBlocks ? _opponent.BlockValue : 0;
            
                var dodgeThrow = Random.Range(100, 0);
                Debug.Log(dodgeThrow);
                if (dodgeThrow >= _opponent.DodgeChanceValue)
                {
                    var resultDamage = _player.PowerfulDamageValue - blockValue;

                    if (resultDamage < 0)
                    {
                        resultDamage = 0;
                    }
                    
                    _opponent.Health.Value -= resultDamage;
                    _player.Endurance.Value -= _player.PowerfulDamageEnduranceCost;
                }
                else
                {
                    Debug.Log("Dodge");
                }
                _isOpponentBlocks = false;
                
                _isPlayerPowerfulAttackPrepared = false;
            }
            else
            {
                Debug.Log("Player Powerful Attack Not Prepared ");
            }
        }
        
        [Button]
        public void OpponentPowerfulAttack()
        {
            if (_isOpponentPowerfulAttackPrepared)
            {
                int blockValue = _isPlayerBlocks ? _player.BlockValue : 0;
            
                var dodgeThrow = Random.Range(100, 0);
                Debug.Log(dodgeThrow);
                if (dodgeThrow >= _player.DodgeChanceValue)
                {
                    var resultDamage = _opponent.PowerfulDamageValue - blockValue;

                    if (resultDamage < 0)
                    {
                        resultDamage = 0;
                    }
                    
                    _player.Health.Value -= resultDamage;
                    _opponent.Endurance.Value -= _opponent.PowerfulDamageEnduranceCost;
                }
                else
                {
                    Debug.Log("Dodge");
                }
                _isPlayerBlocks = false;
                
                _isOpponentPowerfulAttackPrepared = false;
            }
            else
            {
                Debug.Log("Opponent Powerful Attack Not Prepared ");
            }
        }
        
        [Button]
        public void GiveUp()
        {
            _player.Health.Value = 0;
        }
        
        [Button]
        public void PlayerSkipTurn()
        {
            Debug.Log("Player skip turn");
        }
        
        [Button]
        public void OpponentSkipTurn()
        {
            Debug.Log("Opponent skip turn");
        }
        
        [Button]
        public void PlayerPreparePowerfulAttack()
        {
            _isPlayerPowerfulAttackPrepared = true;
            PlayerSkipTurn();
            Debug.Log("Player Prepare Powerful Attack");
        }
        
        [Button]
        public void OpponentPreparePowerfulAttack()
        {
            _isOpponentPowerfulAttackPrepared = true;
            OpponentSkipTurn();
            Debug.Log("Opponent Prepare Powerful Attack");
        }
        
        public void CheckPlayerHealthStats(int value)
        {
            Debug.Log("Check Player Health Stats: " + _player.Health.Value);
            if (_player.Health.Value <= 0)
            {
                Lose();
            }
        }

        public void CheckPlayerEnduranceStats(int value)
        {
            Debug.Log("Check Player Endurance Stats: " + _player.Endurance.Value);
            if (_player.Endurance.Value <= 0)
            {
                Lose();
            }
        }
        
        public void CheckOpponentHealthStats(int value)
        {
            Debug.Log("Check Opponent Health Stats: " + _opponent.Health.Value);
            if (_opponent.Health.Value <= 0)
            {
                Win();
            }
        }
        
        public void CheckOpponentEnduranceStats(int value)
        {
            Debug.Log("Check Opponent Endurance Stats: " + _opponent.Endurance.Value);
            if (_opponent.Endurance.Value <= 0)
            {
                Win();
            }
        }
        
        [Button]
        public void Win()
        {
            Debug.Log("Player Win");
        }
        
        [Button]
        public void Lose()
        {
            Debug.Log("Player Lose");
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}