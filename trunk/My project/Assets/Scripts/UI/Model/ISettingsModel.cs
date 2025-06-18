using UniRx;

namespace UI.Model
{
    public interface ISettingsModel
    {
        IReadOnlyReactiveProperty<bool> MusicNotMuted { get; }
        IReadOnlyReactiveProperty<bool> SoundNotMuted { get; }
        void SetMusicVolume(bool value);
        void SetAudioVolume(bool value);
    }
}