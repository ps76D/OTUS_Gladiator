using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.BattleSystem;
using GameEngine.MessagesSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;


namespace GameEngine
{
    public class BattleSceneView : SceneContentView
    {
        [Inject]
        [SerializeField] private BattleService _battleService;
        
        [SerializeField] private Transform _messagesPlayerRoot;
        [SerializeField] private Transform _messagesOpponentRoot;
        

        [SerializeField] private MessageView _messageViewPrefab;
        [SerializeField] private MessagesDatabase _messagesDatabase;
        
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Animator _opponentAnimator;
        
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
            _battleService.OnPlayerAttack += PlayerAttack;
            _battleService.OnOpponentAttack += OpponentAttack;
            _battleService.OnPlayerDead += PlayerDead;
            _battleService.OnOpponentDead += OpponentDead;
            
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
            _battleService.OnPlayerAttack -= PlayerAttack;
            _battleService.OnOpponentAttack -= OpponentAttack;
            _battleService.OnPlayerDead -= PlayerDead;
            _battleService.OnOpponentDead -= OpponentDead;
        }

        private IEnumerator PlayAnimation(Animator animator, string animationName, float duration)
        {
            animator.Play(animationName);
            yield return new WaitForSeconds(duration);
        }
        
        [Button]
        public void PlayerAttack()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, "Attack", 1f));
            /*_battleService.PlayerAttack();*/
        }
        
        [Button]
        public void OpponentAttack()
        {
            StartCoroutine(PlayAnimation(_opponentAnimator, "Attack", 1f));
            /*_battleService.OpponentAttack();*/
        }
        
        private void PlayerBlock()
        {
            _playerAnimator.Play("Block");
            
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
            _playerAnimator.Play("Dodge");
            
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
        
        public void PlayerDead()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, "Dying", 1f));
            /*_battleService.PlayerAttack();*/
        }
        
        public void OpponentDead()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, "Dying", 1f));
            /*_battleService.PlayerAttack();*/
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