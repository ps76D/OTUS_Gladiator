using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "StatInfoData", menuName = "Stats/StatInfoData", order = 0)]
    public sealed class StatInfoData : ScriptableObject
    {
        [SerializeField] private string _statName;
        [SerializeField] private Sprite _statIcon;

        public string StatName => _statName;
        public Sprite StatIcon=> _statIcon;
    }
}