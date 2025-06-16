using System;
using Sirenix.OdinInspector;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterStat
    {
        public event Action<int> OnStatValueChanged;
        public event Action<int> OnStatExperienceChanged;
        public event Action<int> OnStatRequiredExperienceChanged;
        

        [ShowInInspector, ReadOnly]
        public string Name { get; set; }

        [ShowInInspector, ReadOnly]
        public int Value { get; private set; }
        
        [ShowInInspector]
        public int CurrentStatExperience { get; private set; }

        //TODO Сделать формулу для расчета роста требуемого опыта стата для его левелапа
        [ShowInInspector]
        public int RequiredStatExperience => 1 * (Value + 1);
        
        /*[ShowInInspector, ReadOnly]
        public Sprite Icon { get; private set; }*/

        [Button]
        public void AddStatExperience(int range)
        {
            var xp = Math.Min(CurrentStatExperience + range, RequiredStatExperience);
            CurrentStatExperience = xp;
            OnStatExperienceChanged?.Invoke(xp);
        }
        
        [Button]
        public void ChangeValue(int value)
        {
            Value = value;
            OnStatValueChanged?.Invoke(value);
        }
        
        [Button]
        public void SetCurrentStatExperience(int value)
        {
            CurrentStatExperience = value;
            OnStatExperienceChanged?.Invoke(value);
        }
        
        /*[Button]
        public void SetRequiredStatExperience(int value)
        {
            RequiredStatExperience = value;
            OnStatRequiredExperienceChanged?.Invoke(value);
        }*/
        
        [Button]
        public void IncreaseValue()
        {
            if (!CanIncreaseStatValue()) return;
            CurrentStatExperience = 0;
            Value++;
            OnStatValueChanged?.Invoke(Value);
            OnStatRequiredExperienceChanged?.Invoke(RequiredStatExperience);
            OnStatExperienceChanged?.Invoke(CurrentStatExperience);
        }

        public bool CanIncreaseStatValue()
        {
            return CurrentStatExperience == RequiredStatExperience;
        }
        
        /*[Button]
        public void ChangeIcon(Sprite sprite)
        {
            Icon = sprite;
        }*/
    }
}