using System;
using GameManager;
using TMPro;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UnitWidget : UIScreen
    {
        [SerializeField] private MatchMakingService _matchMakingService;
        
        [SerializeField] private Image _unitPortrait;
        [SerializeField] private GameObject _selector;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _powerText;
        [SerializeField] private GameObject _completedLabel;
        
        [SerializeField] private Button _selectUnitButton;
        
        public Action<UnitWidget> OnSelected;
        public bool IsSelected => _selector.activeSelf;
        
        private IUnitModel _unitModel;
        public IUnitModel UnitModel => _unitModel;
        public Image UnitPortrait => _unitPortrait;
        
        public void SetupWidget(IUnitModel viewModel)
        {
            _unitModel = viewModel;

            _matchMakingService = viewModel.MatchMakingService;
            
            _titleText.text = viewModel.CharacterInfoSObj.CharacterName;
            _descriptionText.text = viewModel.CharacterInfoSObj.CharacterDescription;
            
            _selectUnitButton.onClick.AddListener(OnUnitSelected);
            _selector.SetActive(false);
        }

        private void OnUnitSelected()
        {
            if (!_matchMakingService.IsOpponentSelected.Value)
            {
                _unitModel.SelectOpponent();
            
                OnSelected?.Invoke(this);
            }
            else
            {
                if (_matchMakingService.CurrentOpponent == _unitModel.CharacterInfoSObj)
                {
                    _matchMakingService.DeselectOpponent();
                    
                    OnSelected?.Invoke(this);
                }
                else
                {
                    _unitModel.SelectOpponent();
            
                    OnSelected?.Invoke(this);
                }
            }
            
            
            /*
            _unitModel.SelectOpponent();
            
            OnSelected?.Invoke(this);*/
        }

        public void SetSelected(bool isSelected)
        {
            _selector.SetActive(isSelected);
        }

        public void OnDestroy()
        {
            _selectUnitButton.onClick.RemoveListener(OnUnitSelected);

        }
    }
}