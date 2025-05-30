using System;
using Sirenix.OdinInspector;
using UniRx;

namespace GameEngine.CharacterSystem.StatsSystem
{
    public sealed class CharacterStat
    {
        public event Action<int> OnValueChanged; 

        [ShowInInspector, ReadOnly]
        public string Name { get; set; }

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<int> Value { get; private set; } = new ReactiveProperty<int>();

        [Button]
        public void ChangeValue(int value)
        {
            Value.Value = value;
            OnValueChanged?.Invoke(value);
        }
    }
}