using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace GameEngine.CharacterSystem.StatsSystem
{
    [Serializable]
    public sealed class CharacterStat
    {
        public event Action<int> OnValueChanged; 

        [ShowInInspector, ReadOnly]
        public string Name { get; set; }

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<int> Value { get; private set; } = new ReactiveProperty<int>();
        
        [ShowInInspector, ReadOnly]
        public IReactiveProperty<Sprite> Icon { get; private set; } = new ReactiveProperty<Sprite>();

        [Button]
        public void ChangeValue(int value)
        {
            Value.Value = value;
            OnValueChanged?.Invoke(value);
        }
        
        [Button]
        public void ChangeIcon(Sprite sprite)
        {
            Icon.Value = sprite;
        }
    }
}