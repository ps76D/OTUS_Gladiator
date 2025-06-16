using System;
using Sirenix.OdinInspector;

namespace GameEngine.DaySystem
{
    [Serializable]
    public sealed class DayService
    {

        public Action<int> OnDayChanged;
        
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
            /*_actionsService.RecoverAllActions();*/
        }
    }
}