using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatInfoData", menuName = "Stats/StatInfoData", order = 0)]
    public sealed class StatInfoData : ScriptableObject
    {
        [SerializeField] private string _statName;

        public string StatName => _statName;
    }
}