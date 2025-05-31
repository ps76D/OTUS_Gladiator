using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "CharacterInfoDatabase", menuName = "CharacterProfile/CharacterInfoDatabase", order = 0)]
    public sealed class CharacterDatabase : ScriptableObject
    {
        [SerializeField] private List<CharacterInfoSObj> _characterInfoDatabase;
        
        public List<CharacterInfoSObj> CharacterInfoDatabaseSObjs => _characterInfoDatabase;
    }
}