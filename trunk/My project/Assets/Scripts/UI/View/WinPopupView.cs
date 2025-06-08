using UI.Model;
using UnityEngine;

namespace UI
{
    public class WinPopupView : MonoBehaviour
    {
        private IWinPopupModel _viewModel;
        public IWinPopupModel ViewModel => _viewModel;
        
        public void Show(IWinPopupModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
        }
    }
}