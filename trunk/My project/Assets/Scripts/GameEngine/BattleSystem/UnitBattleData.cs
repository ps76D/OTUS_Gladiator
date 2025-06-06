﻿using System;
using Sirenix.OdinInspector;
using UniRx;

namespace GameEngine.BattleSystem
{
    [Serializable]
    public class UnitBattleData
    {
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _health  = new ();
        [ShowInInspector, ReadOnly]
        private int _fullHealth;
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _endurance = new ();
        [ShowInInspector, ReadOnly]
        private int _fullEndurance;
        [ShowInInspector, ReadOnly]
        private int _agility;
        [ShowInInspector, ReadOnly]
        private int _baseDamageValue;
        [ShowInInspector, ReadOnly]
        private int _powerfulDamageValue;
        [ShowInInspector, ReadOnly]
        private int _powerfulDamageEnduranceCost;
        [ShowInInspector, ReadOnly]
        private int _blockValue;
        [ShowInInspector, ReadOnly]
        private int _blockEnduranceCost;
        [ShowInInspector, ReadOnly]
        private float _dodgeChanceValue;
        [ShowInInspector, ReadOnly]
        private float _moralModifier;
        public ReactiveProperty<int>  Health {
            get => _health;
            set => _health = value;
        }
        public int FullHealth {
            get => _fullHealth;
            set => _fullHealth = value;
        }

        public ReactiveProperty<int> Endurance {
            get => _endurance;
            set => _endurance = value;
        }
        public int FullEndurance {
            get => _fullEndurance;
            set => _fullEndurance = value;
        }

        public int Agility {
            get => _agility;
            set => _agility = value;
        }

        public int BaseDamageValue {
            get => _baseDamageValue;
            set => _baseDamageValue = value;
        }
        public int PowerfulDamageValue {
            get => _powerfulDamageValue;
            set => _powerfulDamageValue = value;
        }
        public int BlockEnduranceCostValue {
            get => _blockEnduranceCost;
            set => _blockEnduranceCost = value;
        }
        public int BlockValue {
            get => _blockValue;
            set => _blockValue = value;
        }
        public int PowerfulDamageEnduranceCost {
            get => _powerfulDamageEnduranceCost;
            set => _powerfulDamageEnduranceCost = value;
        }
        
        public float DodgeChanceValue {
            get => _dodgeChanceValue;
            set => _dodgeChanceValue = value;
        }
        
        public float MoralModifier {
            get => _moralModifier;
            set => _moralModifier = value;
        }
    }
}