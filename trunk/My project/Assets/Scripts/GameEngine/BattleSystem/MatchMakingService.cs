using System;
using GameEngine.CharacterSystem;
using GameEngine.CharacterSystem.StatsSystem;
using PlayerProfileSystem;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameEngine.BattleSystem
{
    [Serializable]
    public class MatchMakingService : MonoBehaviour
    {
        [SerializeField] private CharacterDatabase _enemyDatabase;
        
        [Inject]
        private PlayerProfile _playerProfile;
        [Inject]
        private BattleService _battleService;
        
        [SerializeField] private CharacterProfile _playerCharacterProfile;
        [SerializeField] private CharacterProfile _opponentProfile;
        public CharacterDatabase EnemyDatabase => _enemyDatabase;
        public CharacterProfile OpponentProfile => _opponentProfile;

        [Button]
        public void StartMatch()
        {
            SelectPlayerCharacter(_playerProfile.CharacterService.CurrentCharacterProfile); 
            
            float playerMoralModifier = CalcMoralModifierValue(_playerProfile.MoralService);
            
            UnitBattleData playerBattleData = SetUnitBattleData(_playerCharacterProfile.CharacterStatsInfo, playerMoralModifier);
            UnitBattleData opponentBattleData = SetUnitBattleData(_opponentProfile.CharacterStatsInfo,1);
            
            playerBattleData.MoralModifier = playerMoralModifier;
            //TODO можно переделать чтобы в профиле врага была мораль определенного значения. и сделать механику снижения/увеличения его морали перед боем
            opponentBattleData.MoralModifier = 1;
            
            playerBattleData.DodgeChanceValue = CalcDodgeChanceValue(playerBattleData, opponentBattleData, playerMoralModifier);
            opponentBattleData.DodgeChanceValue = CalcDodgeChanceValue(opponentBattleData, playerBattleData, 1);
            
            _battleService.Init(playerBattleData, opponentBattleData);
        }

        [Button]
        public void SelectOpponent(CharacterInfoSObj characterInfo)
        {
            _opponentProfile = new CharacterProfile(characterInfo);
        }

        private UnitBattleData SetUnitBattleData(CharacterStatsInfo statsInfo, float moralModifier)
        {
            int health = CalcHealth(statsInfo);
            int endurance = CalcEndurance(statsInfo);
            int baseDamageValue = CalcBaseDamageValue(statsInfo, moralModifier);
            int powerfulDamageValue = CalcPowerfulDamageValue(statsInfo, moralModifier);
            int powerfulDamageEnduranceCost = PowerfulDamageEnduranceCostValue(statsInfo);
            int blockValue = CalcBlockValue(statsInfo, moralModifier);
            int blockEnduranceCost = BlockEnduranceCostValue(statsInfo);
            
            UnitBattleData unitBattleData = new ()
            {
                Health =  new ReactiveProperty<int>(health),
                FullHealth = health,
                Endurance =  new ReactiveProperty<int>(endurance),
                FullEndurance = endurance,
                Agility = statsInfo.GetStat(StatsNamesConstants.Agility).Value,
                BaseDamageValue = baseDamageValue,
                PowerfulDamageValue = powerfulDamageValue,
                PowerfulDamageEnduranceCost = powerfulDamageEnduranceCost,
                BlockValue = blockValue,
                BlockEnduranceCostValue = blockEnduranceCost,
            };

            return unitBattleData;
        }
        
        private void SelectPlayerCharacter(CharacterProfile characterProfile)
        {
            _playerCharacterProfile = characterProfile;
        }

        private int CalcHealth(CharacterStatsInfo statsInfo)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            
            int health = (int) (strengthValue/3f * 10 + enduranceValue * 10); 
            
            return health;
        }
        
        private int CalcEndurance(CharacterStatsInfo statsInfo)
        {
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            
            int endurance = enduranceValue * 15; 
            
            return endurance;
        }
        
        private int CalcBaseDamageValue(CharacterStatsInfo statsInfo, float moralModifier)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            int agilityValue = statsInfo.GetStat(StatsNamesConstants.Agility).Value;
            
            int baseDamageValue = (int) ((strengthValue + agilityValue/2f) * moralModifier); 
            
            return baseDamageValue;
        }
        
        private int CalcPowerfulDamageValue(CharacterStatsInfo statsInfo, float moralModifier)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            int agilityValue = statsInfo.GetStat(StatsNamesConstants.Agility).Value;
            
            int powerfulDamageValue = (int) ((strengthValue + agilityValue/2f) * 3 * moralModifier); 
            
            return powerfulDamageValue;
        }
        
        private int CalcBlockValue(CharacterStatsInfo statsInfo, float moralModifier)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            int agilityValue = statsInfo.GetStat(StatsNamesConstants.Agility).Value;
            
            int blockValue = (int) ((strengthValue + agilityValue/2f) * 2 * moralModifier); 
            
            return blockValue;
        }
        
        private int BlockEnduranceCostValue(CharacterStatsInfo statsInfo)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            int agilityValue = statsInfo.GetStat(StatsNamesConstants.Agility).Value;
            
            int blockEnduranceCost = (int) (strengthValue + agilityValue/2f); 
            
            return blockEnduranceCost;
        }
        
        private int PowerfulDamageEnduranceCostValue(CharacterStatsInfo statsInfo)
        {
            int strengthValue = statsInfo.GetStat(StatsNamesConstants.Strength).Value;
            int enduranceValue = statsInfo.GetStat(StatsNamesConstants.Endurance).Value;
            int agilityValue = statsInfo.GetStat(StatsNamesConstants.Agility).Value;
            
            int blockEnduranceCost = (int) (strengthValue + agilityValue/2f) * 2; 
            
            return blockEnduranceCost;
        }
        
        private float CalcDodgeChanceValue(UnitBattleData unitBattleData, UnitBattleData opponentBattleData, float moralModifier)
        {
            int agilityValue = unitBattleData.Agility;
            int opponentAgilityValue = opponentBattleData.Agility;
            
            float dodgeChanceValue = 0.25f * agilityValue/opponentAgilityValue * 100 * moralModifier; 
            
            return dodgeChanceValue;
        }
        
        private float CalcMoralModifierValue(MoralService moralService)
        {
            float dodgeChanceValue = moralService.MoralModifier.Value;
            
            return dodgeChanceValue;
        }
    }
}
