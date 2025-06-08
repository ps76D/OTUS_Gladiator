using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UI.Model;
using UnityEngine;

namespace UI
{
    public class MessageView: MonoBehaviour
    {
        [SerializeField] private Animator _moveAnimator;
        
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Color _messageColor;
        
        [SerializeField] private float _timeToDestroy = 1f;
        
        private IMessageModel _viewModel;
        public IMessageModel ViewModel => _viewModel;
        
        public void Show(IMessageModel viewModel)
        {
            _viewModel = viewModel;
            _messageText.text = viewModel.Message;
            _messageColor = viewModel.Color;
            _messageText.color = viewModel.Color;
            _moveAnimator.Play("Message_Spawn");
            
            StartCoroutine(DestroyMessage());
        }

        private IEnumerator DestroyMessage()
        {
            yield return new WaitForSecondsRealtime(_timeToDestroy);
            
            DestroyImmediate(gameObject);
        }
    }
}