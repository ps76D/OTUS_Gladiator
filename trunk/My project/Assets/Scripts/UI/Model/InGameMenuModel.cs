using Infrastructure;
using UI.Infrastructure;

namespace UI.Model
{
    public class InGameMenuModel : IInGameMenuModel
    {
        private readonly UIManager _uiManager;

        public InGameMenuModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void LoadGame()
        {
            _uiManager.ProfileService.InitializeNewGameProfile();
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<LoadInGameState>();
            _uiManager.SaveLoadManager.LoadGame();
        }

        public void SaveGame()
        {
            _uiManager.SaveLoadManager.SaveGame();
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<GameLoopState>();
        }
        
        public void BackToGame()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<GameLoopState>();
        }
        
        public void ToMainMenu()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.Enter<MainMenuState>();
        }
    }
}