using System;
using Sirenix.OdinInspector;
using Zenject;

namespace GameEngine
{
    [Serializable]
    public sealed class DayService
    {
        [Inject]
        private ActionsService _actionsService;
        
        public event Action<int> OnDayChanged;
        
        public int Day => _day;

        [ShowInInspector, ReadOnly]
        private int _day;
        
        [Button]
        public void SetupDay(int day)
        {
            _day = day;
            OnDayChanged?.Invoke(_day);
        }
        
        [Button]
        public void NextDay()
        {
            _day++;
            OnDayChanged?.Invoke(_day);
            _actionsService.RecoverAllActions();
        }
    }
}