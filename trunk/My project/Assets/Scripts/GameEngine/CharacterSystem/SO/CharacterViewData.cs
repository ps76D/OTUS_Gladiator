using System;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    public class CharacterViewData
    {
        [SerializeField] private int _maxLevel;
        [SerializeField] private Sprite _characterBodyImage;
        [SerializeField] private Sprite _characterPortraitImage;

        public int MaxLevel => _maxLevel;
        public Sprite CharacterBodyImage => _characterBodyImage;
        public Sprite CharacterPortraitImage => _characterPortraitImage;
    }
}