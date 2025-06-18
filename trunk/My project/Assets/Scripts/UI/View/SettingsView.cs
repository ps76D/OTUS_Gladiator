using System;
using System.Collections.Generic;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsView : UIScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _fadeCloseButton;
        [SerializeField] private Toggle _musicOnOffToggleButton;
        [SerializeField] private Toggle _soundOnOffToggleButton;
        
        private ISettingsModel _viewModel;
        public ISettingsModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(ISettingsModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _backButton.onClick.AddListener(BackToInGameMenu);
            _fadeCloseButton.onClick.AddListener(BackToInGameMenu);
            
            /*SetMusicToggleState(_viewModel.MusicNotMuted.Value);
            SetSoundToggleState(_viewModel.SoundNotMuted.Value);*/
            
            _disposables.Add(_viewModel.MusicNotMuted.Subscribe(SetMusicToggleState));
            _disposables.Add(_viewModel.SoundNotMuted.Subscribe(SetSoundToggleState));
            
            _musicOnOffToggleButton.onValueChanged.AddListener(MusicOnOff);
            _soundOnOffToggleButton.onValueChanged.AddListener(SoundOnOff);


            

        }

        private void SetMusicToggleState(bool value)
        {
            _musicOnOffToggleButton.isOn = value;
        }
        private void SetSoundToggleState(bool value)
        {
            _soundOnOffToggleButton.isOn = value;
        }
        
        private void MusicOnOff(bool value)
        {
            _viewModel.SetMusicVolume(value);
        }
        
        private void SoundOnOff(bool value)
        {
            _viewModel.SetAudioVolume(value);
        }

        public void Close()
        {            
            _backButton.onClick.RemoveListener(BackToInGameMenu);
            _fadeCloseButton.onClick.RemoveListener(BackToInGameMenu);
            
            _musicOnOffToggleButton.onValueChanged.RemoveListener(MusicOnOff);
            _soundOnOffToggleButton.onValueChanged.RemoveListener(SoundOnOff);

            foreach (var disposable in _disposables)
                disposable.Dispose();
            
            gameObject.SetActive(false);
        }

        private void BackToInGameMenu()
        {
            Close();
        }
    }
}