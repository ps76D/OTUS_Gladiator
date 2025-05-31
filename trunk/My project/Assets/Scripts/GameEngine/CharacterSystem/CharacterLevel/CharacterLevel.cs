using System;
using Sirenix.OdinInspector;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterLevel
    {
        public event Action<int> OnLevelUp;
        public event Action<int> OnExperienceChanged;
        public event Action<int> OnRequiredExperienceChanged;
        
        [ShowInInspector]
        public int CurrentLevel { get; set; } = 1;

        [ShowInInspector]
        public int CurrentExperience { get; set; }

        [ShowInInspector]
        public int RequiredExperience => 100 * (CurrentLevel + 1);

        [Button]
        public void AddExperience(int range)
        {
            var xp = Math.Min(CurrentExperience + range, RequiredExperience);
            CurrentExperience = xp;
            OnExperienceChanged?.Invoke(xp);
        }

        [Button]
        public void LevelUp()
        {
            if (!CanLevelUp()) return;
            
            CurrentExperience = 0;
            CurrentLevel++;
            OnLevelUp?.Invoke(CurrentLevel);
            OnRequiredExperienceChanged?.Invoke(RequiredExperience);
            OnExperienceChanged?.Invoke(CurrentExperience);
        }

        public bool CanLevelUp()
        {
            return CurrentExperience == RequiredExperience;
        }
    }
}