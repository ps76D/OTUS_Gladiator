using System;
using Sirenix.OdinInspector;
using UniRx;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterLevel
    {
        public event Action OnLevelUp;
        public event Action<int> OnExperienceChanged;

        [ShowInInspector, ReadOnly]
        public int CurrentLevel { get; set; } = 1;

        [ShowInInspector, ReadOnly]
        public int CurrentExperience { get; set; }

        [ShowInInspector, ReadOnly]
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
            OnLevelUp?.Invoke();
        }

        public bool CanLevelUp()
        {
            return CurrentExperience == RequiredExperience;
        }
    }
}