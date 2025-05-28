using System;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : UIScreen
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _settingsButton;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartGameButton);
            _loadGameButton.onClick.AddListener(LoadGameButton);
            _saveGameButton.onClick.AddListener(StartGameButton);
            _settingsButton.onClick.AddListener(StartGameButton);
        }

        private void StartGameButton()
        {
            StartGame(new MainMenuModel(_uiManager));
        }
        
        private void StartGame(MainMenuModel model)
        {
            model.OnStartGameButtonClicked?.Invoke();
        }
        
        private void LoadGameButton()
        {
            LoadGame(new MainMenuModel(_uiManager));
        }
        
        private void LoadGame(MainMenuModel model)
        {
            model.OnLoadGameButtonClicked?.Invoke();
        }
    }
}