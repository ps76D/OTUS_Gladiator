using System;
using System.Collections;
using GameEngine;
using GameEngine.MessagesSystem;
using GameManager;
using Infrastructure;
using PlayerProfileSystem;
using SaveSystem;
using UI.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Infrastructure
{
    public sealed class UIManager : MonoBehaviour, ICoroutineRunner
    {
        [Inject]
        private GameBootstrapper _gameBootstrapper;
        
        [Inject]
        private ProfileService _profileService;
        
        [Inject]
        private SaveLoadManager _saveLoadManager;
        
        [Inject]
        private MatchMakingService _matchMakingService;
        
        [Inject]
        private MessagesDatabase _messagesDatabase;
        
        [Inject]
        private BattleService _battleService;
        
        [SerializeField] private MainMenu _mainMenuScreen;
        [SerializeField] private Hud _hud;
        [SerializeField] private InGameMenu _inGameMenu;
        [SerializeField] private MatchmakingView _matchmakingView;
        [SerializeField] private BattleView _battleView;
        [SerializeField] private WinPopupView _winPopupView;
        [SerializeField] private LosePopupView _losePopupView;
        [SerializeField] private SettingsView _settingsView;
        
        public Hud Hud => _hud;
        public MatchmakingView MatchmakingView => _matchmakingView;
        public GameBootstrapper GameBootstrapper => _gameBootstrapper;
        public ProfileService ProfileService => _profileService;
        public SaveLoadManager SaveLoadManager => _saveLoadManager;
        public MatchMakingService MatchMakingService => _matchMakingService;
        public WinPopupView WinPopupView => _winPopupView;
        public LosePopupView LosePopupView => _losePopupView;
        public SettingsView SettingsView => _settingsView;

        public Action OnBackToTraining;

        private void Start()
        {
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuSceneLoaded += ShowMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += HideMainMenu;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += HideInGameMenu;
            
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState += HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<MainMenuState>().OnMainMenuState += HideInGameMenu;
            _gameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState += HideHud;
            _gameBootstrapper.Game.StateMachine.GetState<GameLoopState>().OnGameLoopState += ShowHud;

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState += ShowLosePopup;
            _gameBootstrapper.Game.StateMachine.GetState<WinState>().OnWinState += ShowWinPopup;
            
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

            _gameBootstrapper.Game.StateMachine.GetState<LoseState>().OnLoseState -= ShowLosePopup;
            _gameBootstrapper.Game.StateMachine.GetState<WinState>().OnWinState -= ShowWinPopup;
            
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
        
        public void ShowSettings()
        {
            var viewModel = new SettingsModel(this);
            _settingsView.Show(viewModel);
        }
        
        public void CloseSettings()
        {
            _settingsView.Close();
        }
        

        public void ShowHud()
        {
            var viewModel = new HudModel(this);
            _hud.Show(viewModel);
        }
        
        public void HideHud()
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

        public void ShowMatchmakingScreen()
        {
            var viewModel = new MatchmakingModel(this, _matchMakingService.EnemyDatabase);
            _matchmakingView.Show(viewModel);
        }
        
        /*public void HideMatchmakingScreen()
        {
            _matchmakingView.Close();
            StartCoroutine(DisposeCoroutine((IDisposable) _matchmakingView.ViewModel, 1f));
        }*/
        
        public void ShowBattleScreen()
        {
            var viewModel = new BattleModel(this, _battleService);
            _battleView.Show(viewModel);
        }
        
        public void HideBattleScreen()
        {
            _battleView.Hide();
        }
        
        public void ShowWinPopup()
        {
            _winPopupView.Show(new WinPopupModel(this));
        }
        public void ShowLosePopup()
        {
            _losePopupView.Show(new LosePopupModel(this));
        }
        
        public void HideLosePopup()
        {
            _losePopupView.Close();
            OnBackToTraining?.Invoke();
        }
        
        public void HideWinPopup()
        {
            _winPopupView.Close();
            OnBackToTraining?.Invoke();
        }
        
        private IEnumerator DisposeCoroutine(IDisposable viewModel, float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return null;
            
            viewModel.Dispose(); // Уничтожаем модель
        }
    }
}