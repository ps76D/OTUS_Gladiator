using System;
using Sirenix.OdinInspector;
using UniRx;

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

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<int> CurrentExperience { get; set; } = new ReactiveProperty<int>();

        [ShowInInspector]
        public int RequiredExperience => 100 * (CurrentLevel + 1);

        [Button]
        public void AddExperience(int range)
        {
            var xp = Math.Min(CurrentExperience.Value + range, RequiredExperience);
            CurrentExperience.Value = xp;
            OnExperienceChanged?.Invoke(xp);
        }

        [Button]
        public void LevelUp()
        {
            if (!CanLevelUp()) return;
            
            CurrentExperience.Value = 0;
            CurrentLevel++;
            OnLevelUp?.Invoke(CurrentLevel);
            OnRequiredExperienceChanged?.Invoke(RequiredExperience);
            OnExperienceChanged?.Invoke(CurrentExperience.Value);
        }

        public bool CanLevelUp()
        {
            return CurrentExperience.Value == RequiredExperience;
        }
    }
}