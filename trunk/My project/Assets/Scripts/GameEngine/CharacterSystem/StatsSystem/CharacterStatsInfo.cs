using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.CharacterSystem.StatsSystem;
using Sirenix.OdinInspector;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterStatsInfo
    {
        public event Action<CharacterStat> OnStatAdded;
        public event Action<CharacterStat> OnStatRemoved;
        
        [ShowInInspector] 
        private HashSet<CharacterStat> _stats = new();
        
        public HashSet<CharacterStat> Stats {
            get => _stats;
            set => _stats = value;
        }

        public void AddStat(CharacterStat stat)
        {
            if (_stats.Add(stat))
            {
                OnStatAdded?.Invoke(stat);
            }
        }

        public void RemoveStat(CharacterStat stat)
        {
            if (_stats.Remove(stat))
            {
                OnStatRemoved?.Invoke(stat);
            }
        }

        public CharacterStat GetStat(string name)
        {
            foreach (CharacterStat stat in _stats.Where(stat => stat.Name == name))
            {
                return stat;
            }

            throw new Exception($"StatData {name} is not found!");
        }

        public IEnumerable<CharacterStat> GetStats()
        {
            return _stats.ToArray();
        }

        public IEnumerable<StatTransferData> GetStatTransferData()
        {
            var data = new List<StatTransferData>();
            
            foreach (CharacterStat characterStat in _stats)
            {
                StatTransferData transferData = new StatTransferData()
                {
                    Name = characterStat.Name,
                    Value = characterStat.Value,
                    CurrentStatExperience = characterStat.CurrentStatExperience,
                };
                data.Add(transferData);
            }

            return data;
        }
        
        public void SetStats(CharacterData characterData)
        {
            _stats = new HashSet<CharacterStat>();

            foreach (StatTransferData statTransferData in characterData.Stats)
            {
                CharacterStat stat = new()
                {
                    Name = statTransferData.Name
                };
                stat.ChangeValue(statTransferData.Value);
                stat.SetCurrentStatExperience(statTransferData.CurrentStatExperience);
                _stats.Add(stat);
            }
        }
    }
}