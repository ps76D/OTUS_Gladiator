using UnityEngine;
using UnityEngine.Localization;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "StatInfoData", menuName = "Stats/StatInfoData", order = 0)]
    public sealed class StatInfoData : ScriptableObject
    {
        [SerializeField] private string _statName;
        [SerializeField] private Sprite _statIcon;
        [SerializeField] private Sprite _statExpSliderSprite;
        [SerializeField] private LocalizedString _statNameText;
        [SerializeField] private LocalizedString _statDescriptionText;

        public string StatName => _statName;
        public Sprite StatIcon => _statIcon;
        public Sprite StatExpSliderSprite => _statExpSliderSprite;
        public LocalizedString StatNameText => _statNameText;
        public LocalizedString StatDescriptionText => _statDescriptionText;
        
    }
}