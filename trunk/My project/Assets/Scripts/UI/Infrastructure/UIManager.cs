using System;
using Infrastructure;
using Infrastructure.DI;
using Infrastructure.Listeners;
using PlayerProfileSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Infrastructure
{
    public sealed class UIManager : MonoBehaviour
    {
        [Inject]
        private GameBootstrapper _gameBootstrapper;
        
        [Inject]
        private ProfileService _profileService;
        
        [Inject]
        private SaveLoadManager _saveLoadManager;
        
        [SerializeField] private MainMenu _mainMenuScreen;
        [SerializeField] private Hud _hud;
        /*[SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private HUDScreen _hud;*/

        private Action _mainMenuShowHandler;
        private Action _mainMenuHideHandler;
        private Action _hudShowHandler;
        private Action _hudHideHandler;
        private Action _loseScreenShowHandler;
        private Action _pauseScreenShowHandler;
        
        public GameBootstrapper GameBootstrapper => _gameBootstrapper;
        public ProfileService ProfileService => _profileService;
        public SaveLoadManager SaveLoadManager => _saveLoadManager;

        private void Start()
        {
            _mainMenuShowHandler = () => ShowScreen(_mainMenuScreen);
            _mainMenuHideHandler = () => CloseScreen(_mainMenuScreen);
            _hudShowHandler = () => ShowScreen(_hud);
            _hudHideHandler = () => CloseScreen(_hud);
            /*_loseScreenShowHandler = () => ShowScreen(_loseScreen);
            _pauseScreenShowHandler = () => ShowScreen(_pauseScreen);*/
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuSceneLoaded += _mainMenuShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += _mainMenuHideHandler;
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState += _hudHideHandler;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState += _hudHideHandler;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnGameLoopSceneLoaded += _hudShowHandler;

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState += _loseScreenShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState += _pauseScreenShowHandler;
        }
        
        private void OnDisable()
        {
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuSceneLoaded -= _mainMenuShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState -= _mainMenuHideHandler;
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState -= _hudHideHandler;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState -= _hudHideHandler;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnGameLoopSceneLoaded -= _hudShowHandler;

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState -= _loseScreenShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState -= _pauseScreenShowHandler;
        }
        
        private void ShowScreen(Component screen)
        {
            screen.gameObject.SetActive(true);
        }

        public void CloseScreen(Component screen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            screen.gameObject.SetActive(false);
        }
    }
}