using System;
using System.Collections.Generic;
using UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _activeActionIcon;
        
        private readonly List<IDisposable> _disposables = new();
        
        private IActionModel _viewModel;

        public void Show(IActionModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            
        }
    }
}