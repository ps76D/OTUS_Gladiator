using UI.Model;
using UnityEngine;

namespace UI
{
    public class LosePopupView : MonoBehaviour
    {
        private ILosePopupModel _viewModel;
        public ILosePopupModel ViewModel => _viewModel;
        
        public void Show(ILosePopupModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
        }
    }
}