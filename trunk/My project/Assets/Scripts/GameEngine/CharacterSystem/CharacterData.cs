using System;
using System.Collections.Generic;
using GameEngine.CharacterSystem.StatsSystem;
using UniRx;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public class CharacterData
    {
        public string Guid;
        public string Name;
        public string Description;
        public int CurrentLevel;
        public int CurrentExperience;
        public CharacterStat[] Stats;
    }
}