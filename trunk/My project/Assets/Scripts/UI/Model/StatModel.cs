using GameEngine.CharacterSystem.StatsSystem;
using UniRx;
using UnityEngine;

namespace UI.Model
{
    public class StatModel : IStatModel
    {
        private readonly CharacterStat _characterStat;
        
        public IReadOnlyReactiveProperty<int> StatValue => _statValue;

        public Sprite Icon {
            get;
        }

        private readonly ReactiveProperty<int> _statValue = new(0);

        public StatModel(CharacterStat characterStat)
        {
            _characterStat = characterStat;
            
            UpdateStatValue(_characterStat.Value.Value);
            
            _characterStat.OnValueChanged += UpdateStatValue;
            
            Icon = _characterStat.Icon.Value;
        }

        private void UpdateStatValue(int value)
        {
            _statValue.Value = value;
        }
    }
}