using System;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "CharacterInfoSObj", menuName = "CharacterProfile/CharacterInfoSObj", order = 0)]
    public sealed class CharacterInfoSObj : ScriptableObject
    {
        [SerializeField] private string _characterGuid = Guid.NewGuid().ToString();
        [SerializeField] private string _characterName;
        [SerializeField] private string _characterDescription;
        [SerializeField] private Sprite _characterIcon;
        [SerializeField] private Sprite _characterBattleImage;
        
        [SerializeField] private StatsDatabase _statsDatabase;
        
        [SerializeField] private int _maxActionsCount;
        [SerializeField] private int _rewardForDefeatEnemy;
        
        
        public int MaxActionsCount => _maxActionsCount;

        public string CharacterGuid => _characterGuid;
        public string CharacterName => _characterName;
        public string CharacterDescription => _characterDescription;
        public Sprite CharacterIcon => _characterIcon;
        public Sprite CharacterBattleImage => _characterBattleImage;
        public StatsDatabase StatsDatabase => _statsDatabase;
        public int RewardForDefeatEnemy => _rewardForDefeatEnemy;

    }
}