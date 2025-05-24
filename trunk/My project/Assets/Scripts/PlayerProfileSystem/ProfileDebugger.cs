using UnityEngine;

namespace PlayerProfileSystem
{
    public class ProfileDebugger : MonoBehaviour
    {
        [SerializeField] private ProfileService _manager;
        
        public ProfileService Manager
        {
            get => _manager;
            set => _manager = value;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}