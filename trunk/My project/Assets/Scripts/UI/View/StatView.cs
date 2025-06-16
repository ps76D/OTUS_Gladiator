using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.CharacterSystem;
using TMPro;
using UI.Model;
using UI.SO;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _statValueText;
        [SerializeField] private TMP_Text _statExpValueText;
        [SerializeField] private Slider _statExpSlider;
        [SerializeField] private Image _statExpSliderSprite;
        [SerializeField] private StatsViewDatabase _characterDatabase;
        
        private readonly List<IDisposable> _disposables = new();
        
        private IStatModel _viewModel;

        public void Show(IStatModel viewModel)
        {
            _viewModel = viewModel;
            
            gameObject.SetActive(true);

            SetStatValueData(_viewModel);
            SetExpData(_viewModel);
            
            _disposables.Add(viewModel.StatValue.Subscribe(UpdateStatValueText));
            _disposables.Add(viewModel.CurrentStatExperience.Subscribe(UpdateStatExperienceText));
            _disposables.Add(viewModel.RequiredStatExperience.Subscribe(UpdateStatExperienceText));

            SetStatIcon(viewModel);
        }
        
        private void UpdateStatValueText(int value)
        {
            SetStatValueData(_viewModel);
        }
        
        private void SetStatValueData(IStatModel viewModel)
        {
            _statValueText.text = viewModel.StatValue.ToString();
        }

        private void SetStatIcon(IStatModel viewModel)
        {
            StatData iconData;
            try
            {
                iconData = _characterDatabase.StartStatsDatabase.
                    FirstOrDefault(x => x._statInfoData.StatName == viewModel.CharacterStat.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find StatData for Stat to set his icon");
                Console.WriteLine(e);
                throw;
            }

            if (iconData != null) _icon.sprite = iconData._statInfoData.StatIcon;
            
            if (iconData != null) _statExpSliderSprite.sprite = iconData._statInfoData.StatExpSliderSprite;
        }
        
        private void UpdateStatExperienceText(int exp)
        {
            SetExpData(_viewModel);
        }
        
        private void SetExpData(IStatModel viewModel)
        {
            _statExpValueText.text = viewModel.CurrentStatExperience + " / " + viewModel.RequiredStatExperience;
            
            float sliderValue = (float)viewModel.CurrentStatExperience.Value / viewModel.RequiredStatExperience.Value;
            
            _statExpSlider.value = sliderValue;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}