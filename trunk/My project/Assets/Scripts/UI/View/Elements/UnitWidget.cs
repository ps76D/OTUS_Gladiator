using System;
using TMPro;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UnitWidget : UIScreen
    {
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
            _titleText.text = viewModel.CharacterInfoSObj.CharacterName;
            _descriptionText.text = viewModel.CharacterInfoSObj.CharacterDescription;
            
            _selectUnitButton.onClick.AddListener(OnUnitSelected);
            _selector.SetActive(false);
        }

        private void OnUnitSelected()
        {
            _unitModel.SelectOpponent();
            
            OnSelected?.Invoke(this);
            
            /*bool shouldSelect = !_selector.activeSelf;
            SetSelected(shouldSelect);*/
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