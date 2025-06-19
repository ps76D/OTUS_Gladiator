using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.CharacterSystem;
using GameManager;
using Infrastructure;
using UI.Infrastructure;
using UnityEngine;
using Zenject;
using CharacterInfo = GameEngine.CharacterSystem.CharacterInfo;

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
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<BattleState>();
            
            _makingService.StartMatch();

            _uiManager.StartCoroutine(SwitchToBattleScreen());
        }

        private IEnumerator SwitchToBattleScreen()
        {
            yield return new WaitForSeconds(1f);
            _uiManager.HideHud();
            _uiManager.ShowBattleScreen();
        }

        public void Dispose()
        {
            Debug.Log("Disposing MatchmakingModel");
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}