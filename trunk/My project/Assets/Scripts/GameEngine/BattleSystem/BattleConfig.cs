using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine
{
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "BattleConfig", order = 0)]
    public class BattleConfig : ScriptableObject
    {
        [InfoBox("Time in seconds:")]
        [Header("Anim Times")]
        [SerializeField] private float _attackAnimTime = 1.5f;
        [SerializeField] private float _powerfulAttackAnimTime = 1.5f;
        [SerializeField] private float _dyingAnimTime = 2f;
        /*[SerializeField] private float _dodgeAnimTime;*/
        [SerializeField] private float _blockAnimTime = 0.5f;
        /*[SerializeField] private float _hitAnimTime;*/
        [SerializeField] private float _skipTurnAnimTime = 1f;
        [SerializeField] private float _preparePowerfulAttackAnimTime = 1f;
        
        [Header("Delays")]
        [SerializeField] private float _nextTurnDelayTime = 1.5f;
        
        [SerializeField] private float _winPopupDelayTime = 4f;
        [SerializeField] private float _losePopupDelayTime = 4f;
        
        [Header("Chances")]
        [InfoBox("Chance in percent, for example 60:")]
        [SerializeField] private int _blockChance = 60;
        [SerializeField] private int _simpleAttackChance = 70;

        public float AttackAnimTime => _attackAnimTime;
        public float PowerfulAttackAnimTime => _powerfulAttackAnimTime;
        public float DyingAnimTime => _dyingAnimTime;
        public float BlockAnimTime => _blockAnimTime;
        public float SkipTurnAnimTime => _skipTurnAnimTime;
        public float PreparePowerfulAttackAnimTime => _preparePowerfulAttackAnimTime;
        
        public float NextTurnDelayTime => _nextTurnDelayTime;
        public float WinPopupDelayTime => _winPopupDelayTime;
        public float LosePopupDelayTime => _losePopupDelayTime;
        
        public int BlockChance => _blockChance;
        public int SimpleAttackChance => _simpleAttackChance;


    }
}