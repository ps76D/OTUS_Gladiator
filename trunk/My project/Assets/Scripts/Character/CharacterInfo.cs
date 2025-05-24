using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Character
{
    [Serializable]
    public sealed class CharacterInfo
    {
        public event Action<string> OnNameChanged;
        public event Action<string> OnDescriptionChanged;
        public event Action<Sprite> OnIconChanged;

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<string> Name { get; private set; } = new ReactiveProperty<string>();

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<string> Description { get; private set; } = new ReactiveProperty<string>();

        [ShowInInspector, ReadOnly]
        public IReactiveProperty<Sprite> Icon { get; private set; } = new ReactiveProperty<Sprite>();

        [Button]
        public void ChangeName(string name)
        {
            Name.Value = name;
            OnNameChanged?.Invoke(name);
        }

        [Button]
        public void ChangeDescription(string description)
        {
            Description.Value = description;
            OnDescriptionChanged?.Invoke(description);
        }

        [Button]
        public void ChangeIcon(Sprite icon)
        {
            Icon.Value = icon;
            OnIconChanged?.Invoke(icon);
        }
    }
}