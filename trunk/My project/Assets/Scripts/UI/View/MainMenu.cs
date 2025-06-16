using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : UIScreen
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _settingsButton;
        
        private IMainMenuModel _viewModel;
        public IMainMenuModel ViewModel => _viewModel;

        public void Show(IMainMenuModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _startGameButton.onClick.AddListener(StartGameButton);
            _loadGameButton.onClick.AddListener(LoadGameButton);
            _settingsButton.onClick.AddListener(SettingsGameButton);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _startGameButton.onClick.RemoveListener(StartGameButton);
            _loadGameButton.onClick.RemoveListener(LoadGameButton);
            _settingsButton.onClick.RemoveListener(SettingsGameButton);
        }

        private void StartGameButton()
        {
            _viewModel.StartGame();
        }
        
        private void LoadGameButton()
        {
            _viewModel.LoadGame();
        }
        
        private void SettingsGameButton()
        {
            _viewModel.OpenSettings();
        }

    }
}