using GameEngine.CharacterSystem;
using GameEngine.CharacterSystem.StatsSystem;
using UniRx;
using UnityEngine;

namespace UI.Model
{
    public class StatModel : IStatModel
    {
        private readonly CharacterStat _characterStat;
        public  CharacterStat  CharacterStat => _characterStat;
        public IReadOnlyReactiveProperty<int> StatValue => _statValue;
        private readonly ReactiveProperty<int> _statValue = new(0);
        
        public IReadOnlyReactiveProperty<int> CurrentStatExperience => _currentStatExperience;
        private readonly ReactiveProperty<int> _currentStatExperience = new(0);
        public IReadOnlyReactiveProperty<int> RequiredStatExperience => _requiredStatExperience;
        private readonly ReactiveProperty<int> _requiredStatExperience = new(0);

        public StatModel(CharacterStat characterStat)
        {
            _characterStat = characterStat;
            
            UpdateStatValue(_characterStat.Value);
            UpdateStatExpValue(_characterStat.CurrentStatExperience);
            UpdateStatRequiredExpValue(_characterStat.RequiredStatExperience);
            
            _characterStat.OnStatValueChanged += UpdateStatValue;
            _characterStat.OnStatExperienceChanged += UpdateStatExpValue;
            _characterStat.OnStatRequiredExperienceChanged += UpdateStatRequiredExpValue;
        }

        private void UpdateStatValue(int value)
        {
            _statValue.Value = value;
        }
        
        private void UpdateStatExpValue(int value)
        {
            _currentStatExperience.Value = value;
        }
        
        private void UpdateStatRequiredExpValue(int value)
        {
            _requiredStatExperience.Value = value;
        }
    }
}