using System;
using UnityEngine;
using Zenject;

namespace PlayerProfileSystem
{
    [Serializable]
    public class ProfileService
    {
        [Inject]
        [SerializeField] private PlayerProfile _playerProfile;
        
        public ProfileService()
        {
            ProfileDebuggerInitialize();
        }

        private void ProfileDebuggerInitialize()
        {
#if UNITY_EDITOR
            new GameObject("Profile Debugger").AddComponent<ProfileDebugger>().Manager = this;
#endif
        }
    }
}