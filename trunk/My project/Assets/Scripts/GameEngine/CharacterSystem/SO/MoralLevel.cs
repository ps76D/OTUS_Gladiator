using System;
using UnityEngine;
using UnityEngine.Localization;

namespace GameEngine.CharacterSystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "MoralLevel", menuName = "Moral/MoralLevel", order = 0)]
    public class MoralLevel : ScriptableObject
    {
        [SerializeField] private int _moralLevelValue;
        [SerializeField] private float _moralModifier;
        [SerializeField] private LocalizedString _moralLevelText;
        
        public int MoralLevelValue => _moralLevelValue;
        public float MoralModifier => _moralModifier;

        public LocalizedString MoralLevelText => _moralLevelText;
    }
}