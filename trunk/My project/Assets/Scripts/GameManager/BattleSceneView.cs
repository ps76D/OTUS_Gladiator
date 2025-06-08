using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.BattleSystem;
using Sirenix.OdinInspector;
using UI;
using UI.Model;
using UI.SO;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameManager
{
    public class BattleSceneView : SceneView
    {
        [Inject]
        [SerializeField] private BattleService _battleService;
        
        [SerializeField] private Transform _messagesPlayerRoot;
        [SerializeField] private Transform _messagesOpponentRoot;
        
        [SerializeField] private MessageView _messageViewPrefab;
        [SerializeField] private MessagesDatabase _messagesDatabase;
        
        private readonly List<Action> _messagesPull = new ();

        
        private bool _isDisplaying;
        
        private void Start()
        {
            _battleService.OnPlayerTakeDamage += PlayerTakeDamage;
            _battleService.OnOpponentTakeDamage += OpponentTakeDamage;
            _battleService.OnPlayerDodge += PlayerDodge;
            _battleService.OnOpponentDodge += OpponentDodge;
            _battleService.OnPlayerBlocked += PlayerBlock;
            _battleService.OnOpponentBlocked += OpponentBlock;
            _battleService.OnPlayerEnduranceSpent += PlayerEnduranceSpent;
            _battleService.OnOpponentEnduranceSpent += OpponentEnduranceSpent;
            
            /*_uiManager.Hud.OnStrengthIncreased += ShowMessageIncreaseStrength;
            _uiManager.Hud.OnEnduranceIncreased += ShowMessageIncreaseEndurance;
            _uiManager.Hud.OnAgilityIncreased += ShowMessageIncreaseAgility;
            _uiManager.Hud.OnLevelUp += ShowMessageLevelUp;
            _uiManager.Hud.OnMoralChanged += ShowMessageMoralChanged;*/
            
            _messagesPull.Clear();
        }

        private void OnDisable()
        {
            _battleService.OnPlayerTakeDamage -= PlayerTakeDamage;
            _battleService.OnOpponentTakeDamage -= OpponentTakeDamage;
            _battleService.OnPlayerDodge -= PlayerDodge;
            _battleService.OnOpponentDodge -= OpponentDodge;
            _battleService.OnPlayerBlocked -= PlayerBlock;
            _battleService.OnOpponentBlocked -= OpponentBlock;
            _battleService.OnPlayerEnduranceSpent -= PlayerEnduranceSpent;
            _battleService.OnOpponentEnduranceSpent -= OpponentEnduranceSpent;
        }

        private void PlayerBlock()
        {
            MessageModel message = BlockMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot);
        }
        
        private void OpponentBlock()
        {
            MessageModel message = BlockMessage();
            CollectAndShowMessages(message, _messagesOpponentRoot);
        }
        
        private void PlayerDodge()
        {
            MessageModel message = DodgeMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot);
        }
        
        private void OpponentDodge()
        {
            MessageModel message = DodgeMessage();
            CollectAndShowMessages(message, _messagesOpponentRoot);
        }
        
        private void PlayerTakeDamage(int damage)
        {
            MessageModel message = DamageMessage(damage);
            CollectAndShowMessages(message, _messagesPlayerRoot);
        }
        
        private void OpponentTakeDamage(int damage)
        {
            MessageModel message = DamageMessage(damage);
            CollectAndShowMessages(message, _messagesOpponentRoot);
        }
        
        private void PlayerEnduranceSpent(int value)
        {
            MessageModel message = EnduranceSpentMessage(value);
            CollectAndShowMessages(message, _messagesPlayerRoot);
        }
        private void OpponentEnduranceSpent(int value)
        {
            MessageModel message = EnduranceSpentMessage(value);
            CollectAndShowMessages(message, _messagesOpponentRoot);
        }
        
        
        [Button]
        private void ShowMessageDamage(int value, Transform messageParent)
        {
            MessageModel message = DamageMessage(value);
            CollectAndShowMessages(message, messageParent);
        }
        [Button]
        private void ShowMessageDodge(Transform messageParent)
        {
            MessageModel message = DodgeMessage();
            CollectAndShowMessages(message, messageParent);
        }
        [Button]
        private void ShowMessageBlock(Transform messageParent)
        {
            MessageModel message = BlockMessage();
            CollectAndShowMessages(message, messageParent);
        }
        
        private MessageModel EnduranceSpentMessage(int value)
        {
            MessageModel messageModel = new (_messagesDatabase.EnduranceSpent, Color.cyan, value.ToString());
            return messageModel;
        }
        
        private MessageModel DamageMessage(int value)
        {
            MessageModel messageModel = new (_messagesDatabase.Damage, Color.red, value.ToString());
            return messageModel;
        }
        
        private MessageModel DodgeMessage()
        {
            MessageModel messageModel = new (_messagesDatabase.Dodge, Color.blue, "");
            return messageModel;
        }
        
        private MessageModel BlockMessage()
        {
            MessageModel messageModel = new (_messagesDatabase.Block, Color.green, "");
            return messageModel;
        }
        
        [Button]
        private void ShowMessage(MessageModel messageModel, Transform messageParent)
        {
            MessageView messageView = Instantiate(_messageViewPrefab, messageParent);
            messageView.Show(messageModel);
        }

        private IEnumerator ShowMessagesPull()
        {
            _isDisplaying = true;
            
            yield return null;

            foreach (var action in _messagesPull)
            {
                action?.Invoke();
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            _messagesPull.Clear();
            
            _isDisplaying = false;
        }
        
        private void CollectAndShowMessages(MessageModel message, Transform messageParent)
        {
            void Action() => ShowMessage(message, messageParent);
            _messagesPull.Add(Action);
            
            if (!_isDisplaying)
            {
                StartCoroutine(ShowMessagesPull());
            } 
        }
    }
}