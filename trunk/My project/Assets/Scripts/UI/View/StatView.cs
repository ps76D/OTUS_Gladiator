using System;
using System.Collections.Generic;
using TMPro;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _statValue;
        [SerializeField] private TMP_Text _statExpValue;
        [SerializeField] private Slider _statExpSlider;
        
        private readonly List<IDisposable> _disposables = new();
        
        public Image Icon => _icon;
        
        private IStatModel _viewModel;
        public IStatModel ViewModel {
            get => _viewModel;
            set => _viewModel = value;
        }

        public void Show(IStatModel viewModel)
        {
            _viewModel = viewModel;
            
            gameObject.SetActive(true);
            
            _disposables.Add(viewModel.StatValue.Subscribe(UpdateStatValueText));
            
            _icon.sprite = viewModel.Icon;
            
            /*_disposables.Add(viewModel.DayCount.Subscribe(UpdateDayText));
            _disposables.Add(viewModel.MoneyCount.Subscribe(UpdateMoneyText));
            _disposables.Add(viewModel.ExpCount.Subscribe(UpdateExpText));
            _disposables.Add(viewModel.LevelCount.Subscribe(UpdateLevelText));*/


        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
        
        private void UpdateStatValueText(int value)
        {
            SetStatValueData(_viewModel);
        }
        
        private void SetStatValueData(IStatModel viewModel)
        {
            _statValue.text = viewModel.StatValue.ToString();
            /*_statExpValue.text = viewModel.StatValue.ToString();*/
            
            //TODO Stats Experience
            /*float sliderValue = (float)viewModel.ExpCount.Value / _viewModel.RequiredExpCount.Value;
            
            _statExpSlider.value = sliderValue;*/
        }
    }
}