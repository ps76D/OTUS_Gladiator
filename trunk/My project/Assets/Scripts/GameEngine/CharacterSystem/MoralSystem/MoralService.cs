using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.Localization;
using Zenject;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public class MoralService
    {
        [Inject]
        private MoralConfig _moralConfig;
        
        public MoralConfig MoralConfig => _moralConfig;
        
        
        public event Action<int> OnCurrentMoralChanged;
        public event Action<int> OnMaxMoralCountChanged;
        
        public IReactiveProperty<int> CurrentMoral  => _currentMoral;
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _currentMoral = new ();
        
        public IReactiveProperty<int> MaxMoralCount => _maxMoralCount;
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _maxMoralCount = new ();
        
        [Button]
        public void SetupCurrentMorals(int currentMoral)
        {
            _currentMoral.Value = currentMoral;
        }
        
        [Button]
        public void SetupMaxMoralCount(int maxMoralCount)
        {
            _maxMoralCount.Value = maxMoralCount;
        }
        
        [Button]
        public void DegreaseMoral(int moralCount)
        {
            if (_currentMoral.Value > 0)
            {
                _currentMoral.Value -= moralCount;
                
                if (_currentMoral.Value < 0)
                {
                    _currentMoral.Value = 0;
                }
            }
            else
            {
                _currentMoral.Value = 0;
            }
            OnCurrentMoralChanged?.Invoke(_currentMoral.Value);
        }
        
        [Button]
        public void IncreaseMoral(int moralCount)
        {
            if (_currentMoral.Value < _maxMoralCount.Value)
            {
                _currentMoral.Value += moralCount;
                
                if (_currentMoral.Value > _maxMoralCount.Value)
                {
                    _currentMoral.Value = _maxMoralCount.Value;
                }
            }
            else
            {
                _currentMoral.Value = _maxMoralCount.Value;
            }
            OnCurrentMoralChanged?.Invoke(_currentMoral.Value);
        }
        
        
        [Button]
        public void RecoverAllMoral()
        {
            _currentMoral.Value = _maxMoralCount.Value;
            OnCurrentMoralChanged?.Invoke(_currentMoral.Value);
        }
        
        [Button]
        public void IncreaseMaxMoral()
        {
            _maxMoralCount.Value++;
            OnMaxMoralCountChanged?.Invoke(_maxMoralCount.Value);
        }

        public MoralLevel GetMoralLevel()
        {
            foreach (MoralLevel level in MoralConfig._moralLevels)
            {
                int lowerBound = _currentMoral.Value - 10;
                if (level.MoralLevelValue > lowerBound && level.MoralLevelValue <= _currentMoral.Value)
                {
                    return level;
                }
            }

            return MoralConfig._moralLevels[0];
        }
    }
}