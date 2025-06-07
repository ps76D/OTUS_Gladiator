using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitWidget : UIScreen
    {
        [SerializeField] private Image _unitPortrait;
        
        [SerializeField] private Button _selectUnitButton;
        
        private IUnitModel _unitModel;
        public IUnitModel UnitModel => _unitModel;
        public Image UnitPortrait => _unitPortrait;
        

        public void SetupWidget(IUnitModel viewModel)
        {
            _unitModel = viewModel;
            
            _selectUnitButton.onClick.AddListener(SelectUnit);
        }

        private void SelectUnit()
        {
            _unitModel.SelectOpponent();
        }

        public void OnDestroy()
        {
            _selectUnitButton.onClick.RemoveListener(SelectUnit);
        }
    }
}