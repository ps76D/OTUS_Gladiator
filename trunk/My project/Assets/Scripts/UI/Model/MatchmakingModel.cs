using System;
using System.Collections.Generic;
using GameEngine.BattleSystem;
using GameEngine.CharacterSystem;
using UI.Infrastructure;
using Zenject;

namespace UI.Model
{
    public class MatchmakingModel : IMatchmakingModel, IDisposable
    {
        private readonly UIManager _uiManager;
        private readonly CharacterInfo _currentCharacter;
        
        private readonly MatchMakingService _makingService;
        
        [Inject]
        private readonly CharacterDatabase _characterDatabase;
        
        private CharacterInfoSObj _opponentInfoSObj;
        
        private readonly List<IDisposable> _disposables = new();
        
        public MatchMakingService MatchMakingService => _makingService;

        public MatchmakingModel(UIManager uiManager, CharacterDatabase characterDatabase)
        {
            _uiManager = uiManager;
            _characterDatabase = characterDatabase;
            _currentCharacter = uiManager.ProfileService.PlayerProfile.CharacterService.CurrentCharacterProfile.CharacterInfo;
            _makingService = uiManager.MatchMakingService;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        public List<CharacterInfoSObj> GetCharacters()
        {
            return _characterDatabase.CharacterInfoDatabaseSObjs;
        }

        public CharacterInfo GetCurrentCharacter()
        {
            return _currentCharacter;
        }

        public void StartMatch()
        {
            _makingService.StartMatch();
            _uiManager.ShowBattleScreen();
            _uiManager.HideHud();
        }
    }
}