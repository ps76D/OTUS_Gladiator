using System;
using System.Collections.Generic;
using UI.Infrastructure;

namespace UI.Model
{
    public class BattleModel : IBattleModel, IDisposable
    {
        private readonly UIManager _uiManager;
        /*private readonly CharacterInfo _currentCharacter;
        
        private readonly MatchMakingService _makingService;
        
        [Inject]
        private readonly CharacterDatabase _characterDatabase;
        
        private CharacterInfoSObj _opponentInfoSObj;*/
        
        private readonly List<IDisposable> _disposables = new();
        
        public BattleModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            /*_characterDatabase = characterDatabase;
            _currentCharacter = uiManager.ProfileService.PlayerProfile.CharacterService.CurrentCharacterProfile.CharacterInfo;
            _makingService = uiManager.MatchMakingService;*/
        }
        public void Dispose()
        {
            
        }
    }
}