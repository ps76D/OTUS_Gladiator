using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace GameEngine
{
    [Serializable]
    public sealed class DayService
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
            OnDayChanged?.Invoke(_day);
        }
        
        [Button]
        public void NextDay()
        {
            _day++;
            OnDayChanged?.Invoke(_day);
        }
    }
}