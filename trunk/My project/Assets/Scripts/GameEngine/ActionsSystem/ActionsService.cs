using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine
{
    [Serializable]
    public class ActionsService
    {
        public event Action<int> OnAvailableActionsChanged;
        public event Action<int> OnMaxActionsCountChanged;

        public int AvailableActions => _availableActions;
        public int MaxActionsCount => _maxActionsCount;

        [NaughtyAttributes.ReadOnly]
        [ShowInInspector]
        private int _availableActions;
        
        [NaughtyAttributes.ReadOnly]
        [ShowInInspector]
        private int _maxActionsCount;
        
        [Button]
        public void SetupAvailableActions(int availableActions)
        {
            _availableActions = availableActions;
        }
        
        [Button]
        public void SetupMaxActionsCount(int maxActionsCount)
        {
            _maxActionsCount = maxActionsCount;
        }
        
        [Button]
        public void SpendAction(int actionCount)
        {
            if (_availableActions > 0)
            {
                _availableActions -= actionCount;
                
                if (_availableActions < 0)
                {
                    _availableActions = 0;
                }
            }
            else
            {
                _availableActions = 0;
            }
            OnAvailableActionsChanged?.Invoke(_availableActions);
        }
        
        [Button]
        public void AddAction(int actionCount)
        {
            if (_availableActions < _maxActionsCount)
            {
                _availableActions += actionCount;
            }
            else
            {
                _availableActions = _maxActionsCount;
            }
            OnAvailableActionsChanged?.Invoke(_availableActions);
        }
        
        [Button]
        public void RecoverAllActions()
        {
            _availableActions = _maxActionsCount;
            OnAvailableActionsChanged?.Invoke(_availableActions);
        }
        
        [Button]
        public void IncreaseMaxActionsCount()
        {
            _maxActionsCount++;
            OnMaxActionsCountChanged?.Invoke(_maxActionsCount);
        }
    }
}