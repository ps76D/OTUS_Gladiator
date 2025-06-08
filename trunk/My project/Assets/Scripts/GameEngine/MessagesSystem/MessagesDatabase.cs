using UnityEngine;
using UnityEngine.Localization;

namespace GameEngine.MessagesSystem
{
    [CreateAssetMenu(fileName = "MessagesDatabase", menuName = "MessagesDatabase", order = 0)]
    public class MessagesDatabase : ScriptableObject
    {
        [SerializeField] private LocalizedString _moraleChanged;
        [SerializeField] private LocalizedString _strengthIncrease;
        [SerializeField] private LocalizedString _enduranceIncrease;
        [SerializeField] private LocalizedString _agilityIncrease;
        [SerializeField] private LocalizedString _levelUp;
        [SerializeField] private LocalizedString _damage;
        [SerializeField] private LocalizedString _dodge;
        [SerializeField] private LocalizedString _block;
        [SerializeField] private LocalizedString _enduranceSpent;
        public string MoraleChanged => _moraleChanged.GetLocalizedString();
        public string StrengthIncrease => _strengthIncrease.GetLocalizedString();
        public string EnduranceIncrease => _enduranceIncrease.GetLocalizedString();
        public string AgilityIncrease => _agilityIncrease.GetLocalizedString();
        public string LevelUp => _levelUp.GetLocalizedString();
        public string Damage => _damage.GetLocalizedString();
        public string Dodge => _dodge.GetLocalizedString();
        public string Block => _block.GetLocalizedString();
        public string EnduranceSpent => _enduranceSpent.GetLocalizedString();
        
    }
}