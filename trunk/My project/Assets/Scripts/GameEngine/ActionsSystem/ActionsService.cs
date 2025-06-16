using System;
using GameEngine.DaySystem;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace GameEngine.ActionsSystem
{
    [Serializable]
    public class ActionsService : IDisposable
    {
        private DayService _dayService;

        public Action OnCalcMaxActionsCount;
        
        private int _baseMaxActionsCount;
        public int BaseMaxActionsCount {
            get => _baseMaxActionsCount;
            set => _baseMaxActionsCount = value;
        }

        /*[Inject]
        private PlayerProfileDefault _playerProfile;*/
        public event Action<int> OnAvailableActionsChanged;
        public event Action<int> OnMaxActionsCountChanged;

        public ActionsService(DayService dayService)
        {
            _dayService = dayService;

            _dayService.OnDayChanged += CalcMaxActionsCount;
            _dayService.OnDayChanged += RecoverAllActions;
        }

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

        public int CalcMaxActionsCount(int baseValue, int boost)
        {
            int calc= baseValue + boost;
            
            return calc;
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
        
        public void RecoverAllActions(int value)
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

        public void CalcMaxActionsCount(int value)
        {
            OnCalcMaxActionsCount?.Invoke();
        }

        public void Dispose()
        {
            _dayService.OnDayChanged -= CalcMaxActionsCount;
            _dayService.OnDayChanged -= RecoverAllActions;
            

        }
    }
}