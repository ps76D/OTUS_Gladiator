using UnityEngine;

namespace PlayerProfileSystem
{
    [CreateAssetMenu(fileName = "PlayerProfileDefault", menuName = "Common/PlayerProfileDefault")]
    public class PlayerProfileDefault : ScriptableObject
    {
        [SerializeField] private string _profileName;
        [SerializeField] private int _profileDays;

        [SerializeField] private int _money;
        
        /*[SerializeField] private int _maxActionsCount;*/
        
        public string ProfileName => _profileName;
        public int ProfileDays => _profileDays;
        /*public int MaxActionsCount => _maxActionsCount;*/
        
        public int Money => _money;

    }
}
