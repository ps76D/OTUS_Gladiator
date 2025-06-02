using System.Collections.Generic;
using GameEngine.CharacterSystem;
using UnityEngine;

namespace UI.SO
{
    [CreateAssetMenu(fileName = "StatsViewDatabase", menuName = "Stats/StatsViewDatabase", order = 0)]
    public class StatsViewDatabase : ScriptableObject
    {
        [SerializeField] private List<StatData> _startStatsDatabase;
        public IReadOnlyList<StatData> StartStatsDatabase => _startStatsDatabase;
    }
}