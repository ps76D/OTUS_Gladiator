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
        public IEnumerable<StatTransferData> Stats;
        public int AvailableActions;
        public int MaxActionsCount;
        public int BaseMaxActionsCount;
    }
}