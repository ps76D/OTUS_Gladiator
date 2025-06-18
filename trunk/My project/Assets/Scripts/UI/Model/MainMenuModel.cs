using DarkTonic.MasterAudio;
using Infrastructure;
using UI.Infrastructure;
using Zenject;

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
    }
}