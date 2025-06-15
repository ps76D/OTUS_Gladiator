using System;
using Sirenix.OdinInspector;
using UniRx;

namespace GameEngine
{
    [Serializable]
    public class UnitBattleData
    {
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _health  = new ();
        [ShowInInspector, ReadOnly]
        private int _fullHealth;
        [ShowInInspector, ReadOnly]
        private ReactiveProperty<int> _energy = new ();
        [ShowInInspector, ReadOnly]
        private int _fullEnergy;
        [ShowInInspector, ReadOnly]
        private int _agility;
        [ShowInInspector, ReadOnly]
        private int _baseDamageValue;
        [ShowInInspector, ReadOnly]
        private int _powerfulDamageValue;
        [ShowInInspector, ReadOnly]
        private int _powerfulDamageEnergyCost;
        [ShowInInspector, ReadOnly]
        private int _blockValue;
        [ShowInInspector, ReadOnly]
        private int _blockEnergyCost;
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

        public ReactiveProperty<int> Energy {
            get => _energy;
            set => _energy = value;
        }
        public int FullEnergy {
            get => _fullEnergy;
            set => _fullEnergy = value;
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
        public int BlockEnergyCostValue {
            get => _blockEnergyCost;
            set => _blockEnergyCost = value;
        }
        public int BlockValue {
            get => _blockValue;
            set => _blockValue = value;
        }
        public int PowerfulDamageEnergyCost {
            get => _powerfulDamageEnergyCost;
            set => _powerfulDamageEnergyCost = value;
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