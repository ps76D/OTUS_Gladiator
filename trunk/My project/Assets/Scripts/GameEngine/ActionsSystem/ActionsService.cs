using System;
using Sirenix.OdinInspector;
using UniRx;

namespace GameEngine
{
    [Serializable]
    public class ActionsService
    {
        public event Action<int> OnAvailableActionsChanged;
        public event Action<int> OnMaxActionsCountChanged;
        

        public IReactiveProperty<int> AvailableActions  => _availableActions;

        public IReactiveProperty<int> MaxActionsCount => _maxActionsCount;

        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _availableActions = new ();
        
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _maxActionsCount = new ();
        
        [Button]
        public void SetupAvailableActions(int availableActions)
        {
            _availableActions.Value = availableActions;
        }
        
        [Button]
        public void SetupMaxActionsCount(int maxActionsCount)
        {
            _maxActionsCount.Value = maxActionsCount;
        }
        
        [Button]
        public void SpendAction(int actionCount)
        {
            if (_availableActions.Value > 0)
            {
                _availableActions.Value -= actionCount;
                
                if (_availableActions.Value < 0)
                {
                    _availableActions.Value = 0;
                }
            }
            else
            {
                _availableActions.Value = 0;
            }
            OnAvailableActionsChanged?.Invoke(_availableActions.Value);
        }
        
        [Button]
        public void AddAction(int actionCount)
        {
            if (_availableActions.Value < _maxActionsCount.Value)
            {
                _availableActions.Value += actionCount;
            }
            else
            {
                _availableActions.Value = _maxActionsCount.Value;
            }
            OnAvailableActionsChanged?.Invoke(_availableActions.Value);
        }
        
        [Button]
        public void RecoverAllActions()
        {
            _availableActions.Value = _maxActionsCount.Value;
            OnAvailableActionsChanged?.Invoke(_availableActions.Value);
        }
        
        [Button]
        public void IncreaseMaxActionsCount()
        {
            _maxActionsCount.Value++;
            OnMaxActionsCountChanged?.Invoke(_maxActionsCount.Value);
        }
        
        public bool CanSpendAction()
        {
            return _availableActions.Value > 0;
        }
    }
}