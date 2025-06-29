using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public sealed class CharacterInfo
    {
        public string _guid;
        public string _name;
        public string _description;
        public Sprite _icon;
        public Sprite _battleImage;
        
        public event Action<string> OnNameChanged;
        public event Action<string> OnDescriptionChanged;
        public event Action<Sprite> OnIconChanged;
        public event Action<Sprite> OnBattleImageChanged;
        
        [Button]
        public void ChangeName(string name)
        {
            _name = name;
            OnNameChanged?.Invoke(name);
        }

        [Button]
        public void ChangeDescription(string description)
        {
            _description = description;
            OnDescriptionChanged?.Invoke(description);
        }

        [Button]
        public void ChangeIcon(Sprite icon)
        {
            _icon = icon;
            OnIconChanged?.Invoke(icon);
        }
        
        [Button]
        public void ChangeBattleImage(Sprite icon)
        {
            _battleImage = icon;
            OnBattleImageChanged?.Invoke(icon);
        }
    }
}