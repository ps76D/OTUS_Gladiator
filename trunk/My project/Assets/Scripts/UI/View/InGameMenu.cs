using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private Button _toTitleButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _fadeCloseButton;
        
        private IInGameMenuModel _viewModel;
        public IInGameMenuModel ViewModel => _viewModel;

        public void Show(IInGameMenuModel viewModel)
        {
            _viewModel = viewModel;
            
            gameObject.SetActive(true);
            
            _loadButton.onClick.AddListener(LoadGameButtonClicked);
            _saveButton.onClick.AddListener(SaveGameButtonClicked);
            _backButton.onClick.AddListener(BackButtonClicked);
            _fadeCloseButton.onClick.AddListener(BackButtonClicked);
            _toTitleButton.onClick.AddListener(ToTitleButtonClicked);
            _settingsButton.onClick.AddListener(SettingsButtonClicked);

            _loadButton.gameObject.SetActive(PlayerPrefs.HasKey("GameStateKey"));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _loadButton.onClick.RemoveListener(LoadGameButtonClicked);
            _saveButton.onClick.RemoveListener(SaveGameButtonClicked);
            _backButton.onClick.RemoveListener(BackButtonClicked);
            _fadeCloseButton.onClick.RemoveListener(BackButtonClicked);
            _toTitleButton.onClick.RemoveListener(ToTitleButtonClicked);
            _settingsButton.onClick.RemoveListener(SettingsButtonClicked);
        }

        private void SettingsButtonClicked()
        {
            _viewModel.OpenSettings();
        }

        private void LoadGameButtonClicked()
        {
            _viewModel.LoadGame();
        }
        
        private void SaveGameButtonClicked()
        {
            _viewModel.SaveGame();
        }
        
        private void BackButtonClicked()
        {
            _viewModel.BackToGame();
        }
        
        private void ToTitleButtonClicked()
        {
            _viewModel.ToMainMenu();
        }
    }
}