using System.Collections.Generic;
using GameEngine.CharacterSystem;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterInfoDatabase", menuName = "CharacterProfile/CharacterInfoDatabase", order = 0)]
    public sealed class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterInfoSObj> _characterInfoDatabase;
        
        public List<CharacterInfoSObj> CharacterInfoDatabaseSObjs => _characterInfoDatabase;
    }
}