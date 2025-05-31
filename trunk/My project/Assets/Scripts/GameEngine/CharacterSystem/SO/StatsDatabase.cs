using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "StatsDatabase", menuName = "Stats/StatsDatabase", order = 0)]
    public sealed class StatsDatabase : ScriptableObject
    {
        [SerializeField] private List<StatData> _startStatsDatabase;
        public IReadOnlyList<StatData> StartStatsDatabase => _startStatsDatabase;
    }
}