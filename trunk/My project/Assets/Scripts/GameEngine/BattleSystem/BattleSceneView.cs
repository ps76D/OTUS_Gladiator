using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private readonly List<Action> _messagesPlayerPull = new ();
        private readonly List<Action> _messagesOpponentPull = new ();
        
        private bool _isDisplaying;
        private bool _isDisplayingOpponent;
        
        public void OnEnable()
        {
            Cleanup();
            
            StartBattle();
            
            _battleService.OnPlayerTakeDamage += PlayerTakeDamage;
            _battleService.OnOpponentTakeDamage += OpponentTakeDamage;
            _battleService.OnPlayerDodge += PlayerDodge;
            _battleService.OnOpponentDodge += OpponentDodge;
            _battleService.OnPlayerBlocked += PlayerBlock;
            _battleService.OnOpponentBlocked += OpponentBlock;
            _battleService.OnPlayerEnergySpent += PlayerEnergySpent;
            _battleService.OnOpponentEnergySpent += OpponentEnergySpent;
            _battleService.OnPlayerAttack += PlayerAttack;
            _battleService.OnOpponentAttack += OpponentAttack;
            _battleService.OnPlayerDead += PlayerDead;
            _battleService.OnOpponentDead += OpponentDead;
            
            _messagesPlayerPull.Clear();
        }

        private void Cleanup()
        {
            _messagesPlayerPull.Clear();
            
            _battleService.OnPlayerTakeDamage -= PlayerTakeDamage;
            _battleService.OnOpponentTakeDamage -= OpponentTakeDamage;
            _battleService.OnPlayerDodge -= PlayerDodge;
            _battleService.OnOpponentDodge -= OpponentDodge;
            _battleService.OnPlayerBlocked -= PlayerBlock;
            _battleService.OnOpponentBlocked -= OpponentBlock;
            _battleService.OnPlayerEnergySpent -= PlayerEnergySpent;
            _battleService.OnOpponentEnergySpent -= OpponentEnergySpent;
            _battleService.OnPlayerAttack -= PlayerAttack;
            _battleService.OnOpponentAttack -= OpponentAttack;
            _battleService.OnPlayerDead -= PlayerDead;
            _battleService.OnOpponentDead -= OpponentDead;
        }

        private void OnDisable()
        {
            Cleanup();
        }

        private IEnumerator PlayAnimation(Animator animator, string animationName, float duration)
        {
            animator.Play(animationName);
            yield return new WaitForSeconds(duration);
        }
        
        [Button]
        public void PlayerAttack()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, "Attack", _battleService.PlayerAnimTime));
        }
        
        [Button]
        public void OpponentAttack()
        {
            StartCoroutine(PlayAnimation(_opponentAnimator, "Attack", _battleService.PlayerAnimTime));
        }
        
        private void PlayerBlock()
        {
            _playerAnimator.Play("Block");
            
            MessageModel message = BlockMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentBlock()
        {
            _opponentAnimator.Play("Block");
            
            MessageModel message = BlockMessage();
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerDodge()
        {
            _playerAnimator.Play("Dodge");
            
            MessageModel message = DodgeMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentDodge()
        {
            _opponentAnimator.Play("Dodge");
            
            MessageModel message = DodgeMessage();
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerTakeDamage(int damage)
        {
            _playerAnimator.Play("Hit");
            
            MessageModel message = DamageMessage(damage);
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentTakeDamage(int damage)
        {
            _opponentAnimator.Play("Hit");
            
            MessageModel message = DamageMessage(damage);
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerEnergySpent(int value)
        {
            MessageModel message = EnduranceSpentMessage(value);
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        private void OpponentEnergySpent(int value)
        {
            MessageModel message = EnduranceSpentMessage(value);
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }

        private void PlayerDead()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, "Dying", 2f));
        }

        private void OpponentDead()
        {
            StartCoroutine(PlayAnimation(_opponentAnimator, "Dying", 2f));
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
        
        private void ShowMessage(MessageModel messageModel, Transform messageParent)
        {
            MessageView messageView = Instantiate(_messageViewPrefab, messageParent);
            messageView.Show(messageModel);
        }
        
        private void ShowOpponentMessage(MessageModel messageModel, Transform messageParent)
        {
            MessageView messageView = Instantiate(_messageViewPrefab, messageParent);
            messageView.Show(messageModel);
        }

        private IEnumerator ShowMessagesPull(List<Action> messagesPull, float startDelay)
        {
            _isDisplaying = true;
            
            yield return new WaitForSecondsRealtime(startDelay);;

            foreach (var action in messagesPull)
            {
                action?.Invoke();
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            messagesPull.Clear();
            
            _isDisplaying = false;
        }
        
        private IEnumerator ShowOpponentMessagesPull(List<Action> messagesPull, float startDelay)
        {
            _isDisplayingOpponent = true;
            
            yield return new WaitForSecondsRealtime(startDelay);;

            foreach (var action in messagesPull)
            {
                action?.Invoke();
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
            messagesPull.Clear();
            
            _isDisplayingOpponent = false;
        }
        
        private void CollectAndShowMessages(MessageModel message, Transform messageParent, List<Action> messagesPull)
        {
            void Action() => ShowMessage(message, messageParent);
            messagesPull.Add(Action);
        }
        
        private void CollectAndShowOpponentMessages(MessageModel message, Transform messageParent, List<Action> messagesPull)
        {
            void Action() => ShowOpponentMessage(message, messageParent);
            messagesPull.Add(Action);
        }
        
        private void ShowMessages(List<Action> messagesPull)
        {
            if (!_isDisplaying)
            {
                StartCoroutine(ShowMessagesPull(messagesPull, _battleService.PlayerAnimTime/4));
            } 
        }
        
        private void ShowOpponentMessages(List<Action> messagesPull)
        {
            if (!_isDisplayingOpponent)
            {
                StartCoroutine(ShowOpponentMessagesPull(messagesPull, _battleService.PlayerAnimTime/4));
            } 
        }

        private void StartBattle()
        {
            _playerAnimator.Play("Stand");
            _opponentAnimator.Play("Stand");
        }
    }
}