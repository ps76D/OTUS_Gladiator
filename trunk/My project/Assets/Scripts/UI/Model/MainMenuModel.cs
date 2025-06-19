using Infrastructure;
using UI.Infrastructure;
using UnityEngine;

namespace UI.Model
{
    public class MainMenuModel : IMainMenuModel
    {
        private readonly UIManager _uiManager;

        public MainMenuModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            uiManager.MainTheme.StartPlaylist("MainTheme");
        }

        public void StartGame()
        {
            _uiManager.ProfileService.InitializeNewGameProfile();
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
        }
        
        public void LoadGame()
        {
            _uiManager.ProfileService.InitializeNewGameProfile();
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
            _uiManager.SaveLoadManager.LoadGame();
        }
        
        public void OpenSettings()
        {
            _uiManager.ShowSettings();
        }

        public void ExitGame()
        {
#if UNITY_STANDALONE_WIN
            Application.Quit();
#endif
        
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}