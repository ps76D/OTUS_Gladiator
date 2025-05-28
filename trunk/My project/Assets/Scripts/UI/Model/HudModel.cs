using System;
using GameEngine;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class HudModel
    {
        private readonly UIManager _uiManager;
        
        public readonly Action OnEndDayButtonClicked;

        public readonly ReactiveProperty<int> DayCount = new(0);
        
        
        public HudModel(UIManager uiManager, DayService dayService)
        {
            _uiManager = uiManager;
            
            OnEndDayButtonClicked += EndDay;

            dayService.OnDayChanged += UpdateDayCount;
        }
        
        private void EndDay()
        {
            _uiManager.ProfileService.PlayerProfile.DayService.NextDay();

            
        }

        private void UpdateDayCount(int day)
        {
            DayCount.Value = day;
        }
    }
}