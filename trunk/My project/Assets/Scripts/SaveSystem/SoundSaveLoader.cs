using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UniRx;
using UnityEngine;

namespace SaveSystem
{
    public class SoundSaveLoader : MonoBehaviour, IDisposable
    {
        public IReadOnlyReactiveProperty<bool> MusicNotMuted => _musicNotMuted;
        private readonly ReactiveProperty<bool> _musicNotMuted = new();
        public IReadOnlyReactiveProperty<bool> SoundNotMuted => _soundNotMuted;
        private readonly ReactiveProperty<bool> _soundNotMuted = new();
        
        private readonly List<IDisposable> _disposables = new();
        public void Start()
        {

            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            yield return null;
            yield return null;

            CheckSoundInPrefs();
            CheckMusicInPrefs();

            _disposables.Add(_soundNotMuted.Subscribe(SwitchSound));
            _disposables.Add(_musicNotMuted.Subscribe(SwitchMusic));

        }
        
        public void SetMusicOnOff(bool value)
        {
            _musicNotMuted.Value = value;

            if (value)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
        }
        
        public void SetSoundOnOff(bool value)
        {
            _soundNotMuted.Value = value;
            
            if (value)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sound", 0);
            }
        }

        private void SwitchMusic(bool value)
        {
            if (value)
            {
                MasterAudio.UnmuteAllPlaylists();
                Debug.Log("UnmuteAllPlaylists");
            }
            else
            {
                MasterAudio.MuteAllPlaylists();
                Debug.Log("MuteAllPlaylists");
            }
        }

        private void SwitchSound(bool value)
        {
            if (value)
            {
                if (MasterAudio.PlaylistsMuted)
                {
                    MasterAudio.UnmuteEverything();
                    SwitchMusic(false);
                }
                else
                {
                    MasterAudio.UnmuteEverything();
                }

                Debug.Log("UnmuteSounds");
            }
            else
            {
                if (!MasterAudio.PlaylistsMuted)
                {
                    MasterAudio.MuteEverything();
                    SwitchMusic(true);
                }
                else
                {
                    MasterAudio.MuteEverything();
                }
                
                Debug.Log("MuteSounds");
            }
        }

        public void CheckMusicInPrefs()
        {
            if (PlayerPrefs.HasKey("music"))
            {
                if (PlayerPrefs.GetInt("music") == 0)
                {
                    _musicNotMuted.Value = false;
                }
                else
                {
                    _musicNotMuted.Value = true;
                }
            }
            else
            {
                _musicNotMuted.Value = true;
            }

        }
        
        public void CheckSoundInPrefs()
        {
            if (PlayerPrefs.HasKey("sound"))
            {
                if (PlayerPrefs.GetInt("sound") == 0)
                {
                    _soundNotMuted.Value = false;
                }
                else
                {
                    _soundNotMuted.Value = true;
                }
            }
            else
            {
                _soundNotMuted.Value = true;
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}