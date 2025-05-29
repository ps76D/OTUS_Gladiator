using GameEngine;
using Infrastructure;
using PlayerProfileSystem;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class HudModel : IHudModel
    {
        private readonly UIManager _uiManager;
        private readonly DayService _dayService;
        private readonly MoneyStorage _moneyStorage;
        /*private readonly PlayerProfile _playerProfile;*/
        
        public IReadOnlyReactiveProperty<int> DayCount => _dayCount;
        private readonly ReactiveProperty<int> _dayCount = new(0);
        
        public IReadOnlyReactiveProperty<int> MoneyCount => _moneyCount;
        private readonly ReactiveProperty<int> _moneyCount = new(0);
        
        /*private readonly List<IDisposable> _disposables = new();*/
        
        public HudModel(UIManager uiManager)
        {
            /*_playerProfile = playerProfile;*/
            _uiManager = uiManager;
            
            _dayService = uiManager.ProfileService.PlayerProfile.DayService;
            _moneyStorage = uiManager.ProfileService.PlayerProfile.MoneyStorage;
            
            _dayService.OnDayChanged += UpdateDayCount;
            _moneyStorage.OnMoneyChanged += UpdateMoneyCount;
                
            UpdateDayCount(_dayService.Day);
            UpdateMoneyCount(_moneyStorage.Money);
        }

        public void EndDay()
        {
            _dayService.NextDay();
        }

        private void UpdateDayCount(int day)
        {
            _dayCount.Value = day;
        }
        
        private void UpdateMoneyCount(int money)
        {
            _moneyCount.Value = money;
        }
        
        public void InGameMenuShow()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<PauseState>();
        }
        
        /*public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }*/
    }
}