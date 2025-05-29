using Infrastructure;
using UI.Infrastructure;

namespace UI.Model
{
    public class MainMenuModel : IMainMenuModel
    {
        private readonly UIManager _uiManager;

        public MainMenuModel(UIManager uiManager)
        {
            _uiManager = uiManager;
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
        
        
    }
}