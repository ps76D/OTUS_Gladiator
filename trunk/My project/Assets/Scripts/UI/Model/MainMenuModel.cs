using System;
using Infrastructure;
using PlayerProfileSystem;
using UI.Infrastructure;
using UnityEngine;
using Zenject;

namespace UI.Model
{
    public class MainMenuModel
    {
        private readonly UIManager _uiManager;
   

        public readonly Action OnStartGameButtonClicked;
        public readonly Action OnLoadGameButtonClicked;

        public MainMenuModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            OnStartGameButtonClicked += StartGame;
            OnLoadGameButtonClicked += LoadGame;
        }

        private void StartGame()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
            _uiManager.ProfileService.InitializeNewGameProfile();
        }
        
        private void LoadGame()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
            _uiManager.ProfileService.InitializeNewGameProfile();
            _uiManager.SaveLoadManager.LoadGame();
        }
        
    }
}