using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace GameEngine.CharacterSystem.StatsSystem
{
    [Serializable]
    public sealed class CharacterStatsInfo
    {
        public event Action<CharacterStat> OnStatAdded;
        public event Action<CharacterStat> OnStatRemoved;
    
        [ShowInInspector]
        private HashSet<CharacterStat> _stats = new();

        [Button]
        public void AddStat(CharacterStat stat)
        {
            if (_stats.Add(stat))
            {
                OnStatAdded?.Invoke(stat);
            }
        }

        [Button]
        public void RemoveStat(CharacterStat stat)
        {
            if (_stats.Remove(stat))
            {
                OnStatRemoved?.Invoke(stat);
            }
        }

        public CharacterStat GetStat(string name)
        {
            foreach (var stat in _stats)
            {
                if (stat.Name == name)
                {
                    return stat;
                }
            }

            throw new Exception($"StatData {name} is not found!");
        }

        public CharacterStat[] GetStats()
        {
            return _stats.ToArray();
        }
    }
}