using System;
using System.Collections.Generic;
using GameEngine;
using GameEngine.BattleSystem;
using GameEngine.CharacterSystem;
using GameManager;

namespace UI.Model
{
    public class UnitModel : IUnitModel, IDisposable
    {
        private readonly MatchMakingService _matchMakingService;

        private readonly CharacterInfoSObj _characterInfo;
        
        private readonly List<IDisposable> _disposables = new();

        public UnitModel(CharacterInfoSObj characterInfo, MatchMakingService matchMakingService)
        {
            _characterInfo = characterInfo;
            _matchMakingService = matchMakingService;
        }
        
        public void SelectOpponent()
        {
            _matchMakingService.SelectOpponent(_characterInfo);
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}