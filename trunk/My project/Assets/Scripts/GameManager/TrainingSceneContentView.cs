using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.MessagesSystem;
using PlayerProfileSystem;
using Sirenix.OdinInspector;
using UI;
using UI.Infrastructure;
using UI.Model;
using UI.SO;
using UnityEngine;
using Zenject;

namespace GameManager
{
    public class TrainingSceneContentView : SceneContentView
    {
        [Inject]
        [SerializeField] private PlayerProfile _playerProfile;
        
        [Inject]
        [SerializeField] private UIManager _uiManager;
        
        [Inject]
        [SerializeField] private BattleConfig _battleConfig;
        
        [SerializeField] private MessagesDatabase _messagesDatabase;
        
        [SerializeField] private Transform _messagesRoot;
        
        [SerializeField] private MessageView _messageViewPrefab;
        
        private readonly List<Action> _messagesPull = new ();
        
        private bool _isDisplaying;

        private void OnEnable()
        {
            _uiManager.Hud.OnStrengthIncreased += ShowMessageIncreaseStrength;
            _uiManager.Hud.OnEnduranceIncreased += ShowMessageIncreaseEndurance;
            _uiManager.Hud.OnAgilityIncreased += ShowMessageIncreaseAgility;
            _playerProfile.CharacterService.CurrentCharacterProfile.CharacterLevel.OnLevelUp += ShowMessageLevelUp;
            _uiManager.Hud.OnMoralChanged += ShowMessageMoralChanged;
            
            _messagesPull.Clear();
        }

        private void OnDisable()
        {
            _uiManager.Hud.OnStrengthIncreased -= ShowMessageIncreaseStrength;
            _uiManager.Hud.OnEnduranceIncreased -= ShowMessageIncreaseEndurance;
            _uiManager.Hud.OnAgilityIncreased -= ShowMessageIncreaseAgility;
            _playerProfile.CharacterService.CurrentCharacterProfile.CharacterLevel.OnLevelUp -= ShowMessageLevelUp;
            _uiManager.Hud.OnMoralChanged -= ShowMessageMoralChanged;
            
            _messagesPull.Clear();
        }


        [Button]
        private void ShowMessage(MessageModel messageModel)
        {
            MessageView messageView = Instantiate(_messageViewPrefab, _messagesRoot);
            messageView.Show(messageModel);
        }

        private IEnumerator ShowMessagesPull()
        {
            _isDisplaying = true;
            
            yield return null;

            foreach (var action in _messagesPull.ToList())
            {
                action?.Invoke();
                _messagesPull.Remove(action);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            _messagesPull.Clear();
            
            _isDisplaying = false;
        }
        
        private void ShowMessageLevelUp(int i)
        {
            MessageModel message = LevelUpMessage();
            CollectAndShowMessages(message);
        }
        
        private void ShowMessageIncreaseStrength()
        {
            MessageModel message = IncreaseStrengthMessage();
            CollectAndShowMessages(message);
        }
        
        private void ShowMessageIncreaseEndurance()
        {
            MessageModel message = IncreaseEnduranceMessage();
            CollectAndShowMessages(message);
        }
        
        private void ShowMessageIncreaseAgility()
        {
            MessageModel message = IncreaseAgilityMessage();
            CollectAndShowMessages(message);
        }

        private void CollectAndShowMessages(MessageModel message)
        {
            void Action() => ShowMessage(message);
            _messagesPull.Add(Action);
            
            if (!_isDisplaying)
            {
                StartCoroutine(ShowMessagesPull());
            } 
        }

        private void ShowMessageMoralChanged()
        {
            MessageModel message = MoralChangedMessage();
            CollectAndShowMessages(message);
        }
        
        private MessageModel LevelUpMessage()
        {
            var messageModel = new MessageModel(_messagesDatabase.LevelUp, _battleConfig.BaseColor, "" );
            return messageModel;
        }
        
        private MessageModel IncreaseStrengthMessage()
        {
            var messageModel = new MessageModel(_messagesDatabase.StrengthIncrease, _battleConfig.StrengthColor, 1.ToString() );
            return messageModel;
        }
        
        private MessageModel IncreaseEnduranceMessage()
        {
            var messageModel = new MessageModel(_messagesDatabase.EnduranceIncrease, _battleConfig.EnduranceColor, 1.ToString() );
            return messageModel;
        }
        
        private MessageModel IncreaseAgilityMessage()
        {
            var messageModel = new MessageModel(_messagesDatabase.AgilityIncrease, _battleConfig.AgilityColor, 1.ToString() );
            return messageModel;
        }
        
        private MessageModel MoralChangedMessage()
        {
            var newMoralState = _playerProfile.MoralService.GetMoralLevel().MoralLevelText.GetLocalizedString();
            var messageModel = new MessageModel(_messagesDatabase.MoraleChanged, _battleConfig.MoralChangeColor, newMoralState);
            return messageModel;
        }
    }
}
