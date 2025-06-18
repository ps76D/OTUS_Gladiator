using System;
using System.Collections.Generic;
using UI.Infrastructure;
using UniRx;

namespace UI.Model
{
    public class SettingsModel : ISettingsModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        public IReadOnlyReactiveProperty<bool> MusicNotMuted => _musicNotMuted;
        private readonly ReactiveProperty<bool> _musicNotMuted = new();
        public IReadOnlyReactiveProperty<bool> SoundNotMuted => _soundNotMuted;
        private readonly ReactiveProperty<bool> _soundNotMuted = new();
        
        private readonly List<IDisposable> _disposables = new();
        
        public SettingsModel(UIManager uiManager)
        {
            _uiManager = uiManager;
            
            _disposables.Add(_uiManager.SoundSaveLoader.MusicNotMuted.Subscribe(x => _musicNotMuted.Value = x));
            _disposables.Add(_uiManager.SoundSaveLoader.SoundNotMuted.Subscribe(x => _soundNotMuted.Value = x));
            
            
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose(); 
        }

        public void SetMusicVolume(bool value)
        {
            _uiManager.SoundSaveLoader.SetMusicOnOff(value);
        }

        public void SetAudioVolume(bool value)
        {
            _uiManager.SoundSaveLoader.SetSoundOnOff(value);
        }
    }
}