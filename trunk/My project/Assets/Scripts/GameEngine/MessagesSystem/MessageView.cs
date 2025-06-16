using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameEngine.MessagesSystem
{
    public class MessageView: MonoBehaviour
    {
        [SerializeField] private Animator _moveAnimator;
        
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Color _messageColor;
        
        [SerializeField] private float _timeToDestroy = 1f;
        
        private IMessageModel _viewModel;
        public IMessageModel ViewModel => _viewModel;

        private const string MESSAGE_SHOW = "Message_Spawn";
        
        public void Show(IMessageModel viewModel)
        {
            _viewModel = viewModel;
            _messageText.text = viewModel.Message;
            _messageColor = viewModel.Color;
            _messageText.color = viewModel.Color;
            _moveAnimator.Play(MESSAGE_SHOW);

            _messageText.DOFade(1, 0.6f);
            
            StartCoroutine(DestroyMessage());
        }

        private IEnumerator DestroyMessage()
        {
            yield return new WaitForSecondsRealtime(_timeToDestroy);
            
            DestroyImmediate(gameObject);
        }
    }
}