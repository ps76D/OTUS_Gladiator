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
        
        [SerializeField] private SpriteRenderer _playerBattleImage;
        [SerializeField] private SpriteRenderer _opponentBattleImage;
        
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
            StartCoroutine(PlayAnimation(_playerAnimator, BattleConst.ATTACK, _battleService.PlayerAnimTime));
        }
        
        [Button]
        public void OpponentAttack()
        {
            StartCoroutine(PlayAnimation(_opponentAnimator, BattleConst.ATTACK, _battleService.PlayerAnimTime));
        }
        
        private void PlayerBlock()
        {
            _playerAnimator.Play(BattleConst.BLOCK);
            
            MessageModel message = BlockMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentBlock()
        {
            _opponentAnimator.Play(BattleConst.BLOCK);
            
            MessageModel message = BlockMessage();
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerDodge()
        {
            _playerAnimator.Play(BattleConst.DODGE);
            
            MessageModel message = DodgeMessage();
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentDodge()
        {
            _opponentAnimator.Play(BattleConst.DODGE);
            
            MessageModel message = DodgeMessage();
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerTakeDamage(int damage)
        {
            _playerAnimator.Play(BattleConst.HIT);
            
            MessageModel message = DamageMessage(damage);
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        
        private void OpponentTakeDamage(int damage)
        {
            _opponentAnimator.Play(BattleConst.HIT);
            
            MessageModel message = DamageMessage(damage);
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }
        
        private void PlayerEnergySpent(int value)
        {
            MessageModel message = EnergySpentMessage(value);
            CollectAndShowMessages(message, _messagesPlayerRoot, _messagesPlayerPull);
            ShowMessages(_messagesPlayerPull);
        }
        private void OpponentEnergySpent(int value)
        {
            MessageModel message = EnergySpentMessage(value);
            CollectAndShowOpponentMessages(message, _messagesOpponentRoot, _messagesOpponentPull);
            ShowOpponentMessages(_messagesOpponentPull);
        }

        private void PlayerDead()
        {
            StartCoroutine(PlayAnimation(_playerAnimator, BattleConst.DYING, 2f));
        }

        private void OpponentDead()
        {
            StartCoroutine(PlayAnimation(_opponentAnimator, BattleConst.DYING, 2f));
        }
        
        private MessageModel EnergySpentMessage(int value)
        {
            MessageModel messageModel = new (_messagesDatabase.EnduranceSpent, _battleService.BattleConfig.EnergyColor, value.ToString());
            return messageModel;
        }
        
        private MessageModel DamageMessage(int value)
        {
            MessageModel messageModel = new (_messagesDatabase.Damage, _battleService.BattleConfig.DamageColor, value.ToString());
            return messageModel;
        }
        
        private MessageModel DodgeMessage()
        {
            MessageModel messageModel = new (_messagesDatabase.Dodge, _battleService.BattleConfig.DodgeColor, "");
            return messageModel;
        }
        
        private MessageModel BlockMessage()
        {
            MessageModel messageModel = new (_messagesDatabase.Block, _battleService.BattleConfig.BlockColor, "");
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
            _playerBattleImage.sprite = _battleService.Player.Sprite;
            _opponentBattleImage.sprite = _battleService.Opponent.Sprite;
            
            _playerAnimator.Play(BattleConst.STAND);
            _opponentAnimator.Play(BattleConst.STAND);
        }
    }
}