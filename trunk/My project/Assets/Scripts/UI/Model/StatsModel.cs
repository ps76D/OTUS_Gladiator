using System.Collections.Generic;
using GameEngine.CharacterSystem;
using GameEngine.CharacterSystem.StatsSystem;
using UI.Infrastructure;

namespace UI.Model
{
    public class StatsModel : IStatsModel
    {
        private readonly UIManager _uiManager;
        
        private readonly HashSet<StatModel> _stats = new();
        
        public HashSet<StatModel> Stats=> _stats;
        
        /*public IReadOnlyReactiveProperty<int> StatValue => _statValue;
        private readonly ReactiveProperty<int> _statValue = new(0);*/
        
        public StatsModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            CharacterService characterService = uiManager.ProfileService.PlayerProfile.CharacterService;

            CharacterStatsInfo characterStatsInfo = characterService.CurrentCharacterProfile.CharacterStatsInfo;

            var stats = characterStatsInfo.GetStats();
            
            foreach (var stat in stats)
            {
                _stats.Add(new StatModel(stat));
            }
        }
    }
}