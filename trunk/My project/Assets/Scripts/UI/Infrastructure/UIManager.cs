using System;
using Infrastructure;
using PlayerProfileSystem;
using SaveSystem;
using UI.Model;
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
        [SerializeField] private InGameMenu _inGameMenu;
        /*[SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private HUDScreen _hud;*/
        
        private Action _loseScreenShowHandler;
        private Action _pauseScreenShowHandler;
        
        public GameBootstrapper GameBootstrapper => _gameBootstrapper;
        public ProfileService ProfileService => _profileService;
        public SaveLoadManager SaveLoadManager => _saveLoadManager;

        private void Start()
        {
            /*_loseScreenShowHandler = () => ShowScreen(_loseScreen);
            _pauseScreenShowHandler = () => ShowScreen(_pauseScreen);*/
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuSceneLoaded += ShowMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += HideMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += HideInGameMenu;
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState += HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState += HideInGameMenu;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState += HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += ShowHud;

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState += _loseScreenShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState += ShowInGameMenu;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState += HideHud;
        }
        
        private void OnDisable()
        {
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuSceneLoaded -= ShowMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState -= HideMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState -= HideInGameMenu;
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState -= HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState -= HideInGameMenu;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState -= HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState -= ShowHud;

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState -= _loseScreenShowHandler;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState -= ShowInGameMenu;
            _gameBootstrapper.Game.StateMachine.GetState<PauseState>().OnPauseState -= HideHud;
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
        
        private void ShowHud()
        {
            var viewModel = new HudModel(this);
            _hud.Show(viewModel);
        }
        
        private void HideHud()
        {
            _hud.Hide();
        }
        
        private void ShowMainMenu()
        {
            var viewModel = new MainMenuModel(this);
            _mainMenuScreen.Show(viewModel);
        }
        
        private void HideMainMenu()
        {
            _mainMenuScreen.Hide();
        }

        private void ShowInGameMenu()
        {
            var viewModel = new InGameMenuModel(this);
            _inGameMenu.Show(viewModel);
        }
        
        private void HideInGameMenu()
        {
            _inGameMenu.Hide();
        }
    }
}