using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine
{
    [Serializable]
    public class DayService
    {
        public event Action<int> OnDayChanged;

        public int Day => _day;

        [NaughtyAttributes.ReadOnly]
        [ShowInInspector]
        private int _day;
        
        [Button]
        public void SetupDay(int day)
        {
            _day = day;
        }
        
        [Button]
        public void NextDay()
        {
            _day++;
            OnDayChanged?.Invoke(_day);
        }
    }
}