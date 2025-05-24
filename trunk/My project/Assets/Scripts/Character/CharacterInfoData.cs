using System;
using StatSystem;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterInfoData", menuName = "CharacterProfile/CharacterInfoData", order = 0)]
    public sealed class CharacterInfoData : ScriptableObject
    {
        [SerializeField] private string _characterGuid = Guid.NewGuid().ToString();
        [SerializeField] private string _characterName;
        [SerializeField] private string _characterDescription;
        [SerializeField] private Sprite _characterIcon;
        
        [SerializeField] private StatsDatabase _statsDatabase;

        public string CharacterGuid => _characterGuid;
        public string CharacterName => _characterName;
        public string CharacterDescription => _characterDescription;
        public Sprite CharacterIcon => _characterIcon;
        public StatsDatabase StatsDatabase => _statsDatabase;

    }
}