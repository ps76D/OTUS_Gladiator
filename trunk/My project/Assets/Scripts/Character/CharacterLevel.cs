using System;
using Sirenix.OdinInspector;
using UniRx;

namespace Character
{
    [Serializable]
    public sealed class CharacterLevel
    {
        public event Action OnLevelUp;
        public event Action<int> OnExperienceChanged;

        [ShowInInspector, ReadOnly]
        public int CurrentLevel { get; private set; } = 1;

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<int> CurrentExperience { get; set; } = new ReactiveProperty<int>();

        [ShowInInspector, ReadOnly]
        public int RequiredExperience
        {
            get { return 100 * (CurrentLevel + 1); }
        }

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
            OnLevelUp?.Invoke();
        }

        public bool CanLevelUp()
        {
            return CurrentExperience.Value == RequiredExperience;
        }
    }
}