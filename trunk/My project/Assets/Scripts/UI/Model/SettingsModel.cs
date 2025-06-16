using System;
using UI.Infrastructure;

namespace UI.Model
{
    public class SettingsModel : ISettingsModel, IDisposable
    {
        private readonly UIManager _uiManager;
        
        public SettingsModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void Dispose()
        {
            
        }

        public void SetMusicVolume()
        {
            throw new NotImplementedException();
        }

        public void SetAudioVolume()
        {
            throw new NotImplementedException();
        }
    }
}