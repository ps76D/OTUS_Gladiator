using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "CharacterVisualDatabase", menuName = "CharacterVisualDatabase", order = 0)]
    public class CharacterVisualDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterViewData> _characterViews = new();
        
        public List<CharacterViewData> CharacterViews => _characterViews;
    }
}