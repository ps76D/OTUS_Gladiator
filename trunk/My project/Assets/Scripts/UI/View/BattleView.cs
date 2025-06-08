using System;
using System.Collections.Generic;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleView : UIScreen, IDisposable
    {
        [SerializeField] private Slider _playerHealthBar;
        [SerializeField] private Slider _playerEnergyBar;
        [SerializeField] private Slider _opponentHealthBar;
        [SerializeField] private Slider _opponentEnergyBar;
        
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _preparePowerfulAttackButton;
        [SerializeField] private Button _powerfulAttackButton;
        [SerializeField] private Button _blockButton;
        [SerializeField] private Button _skipTurnButton;
        [SerializeField] private Button _giveUpButton;
        
        [SerializeField] private Button _attackOppButton;
        [SerializeField] private Button _preparePowerfulAttackOppButton;
        [SerializeField] private Button _powerfulAttackOppButton;
        [SerializeField] private Button _blockOppButton;
        [SerializeField] private Button _skipTurnOppButton;
        [SerializeField] private Button _giveUpOppButton;
        
        private IBattleModel _viewModel;
        public IBattleModel ViewModel => _viewModel;
        
        private readonly List<IDisposable> _disposables = new();
        
        public void Show(IBattleModel viewModel)
        {
            _viewModel = viewModel;

            gameObject.SetActive(true);
            
            _attackButton.onClick.AddListener(_viewModel.PlayerAttack);
            _preparePowerfulAttackButton.onClick.AddListener(_viewModel.PlayerPrepareAttack);
            _powerfulAttackButton.onClick.AddListener(_viewModel.PlayerPowerfulAttack);
            _blockButton.onClick.AddListener(_viewModel.PlayerBlocks);
            _skipTurnButton.onClick.AddListener(_viewModel.PlayerSkipTurn);
            _giveUpButton.onClick.AddListener(_viewModel.PlayerGiveUp);
            
            _attackOppButton.onClick.AddListener(_viewModel.OpponentAttack);
            _preparePowerfulAttackOppButton.onClick.AddListener(_viewModel.OpponentPrepareAttack);
            _powerfulAttackOppButton.onClick.AddListener(_viewModel.OpponentPowerfulAttack);
            _blockOppButton.onClick.AddListener(_viewModel.OpponentBlocks);
            _skipTurnOppButton.onClick.AddListener(_viewModel.OpponentSkipTurn);
            _giveUpOppButton.onClick.AddListener(_viewModel.OpponentGiveUp);
            
            _disposables.Add(_viewModel.PlayerHealth.Subscribe(UpdatePlayerHealth));
            _disposables.Add(_viewModel.PlayerEnergy.Subscribe(UpdatePlayerEnergy));
            _disposables.Add(_viewModel.OpponentHealth.Subscribe(UpdateOpponentHealth));
            _disposables.Add(_viewModel.OpponentEnergy.Subscribe(UpdateOpponentEnergy));
            
            UpdatePlayerHealth(_viewModel.GetPlayerFullHealth());
            UpdatePlayerEnergy(_viewModel.GetPlayerFullEnergy());
            UpdateOpponentHealth(_viewModel.GetOpponentFullHealth());
            UpdateOpponentEnergy(_viewModel.GetOpponentFullEnergy());
            /*Cleanup();

            SetupEnemyWidgets(_viewModel);
            SetupPlayerWidget(_viewModel);

            _backButton.onClick.AddListener(Close);
            _fadeCloseButton.onClick.AddListener(Close);
            _toBattleButton.onClick.AddListener(ToBattle);*/
        }

        private void UpdatePlayerHealth(int value)
        {
            float sliderValue = (float)_viewModel.PlayerHealth.Value / _viewModel.GetPlayerFullHealth();
            _playerHealthBar.value = sliderValue;
        }
        private void UpdatePlayerEnergy(int value)
        {
            float sliderValue = (float)_viewModel.PlayerEnergy.Value / _viewModel.GetPlayerFullEnergy();
            _playerEnergyBar.value = sliderValue;
        }
        private void UpdateOpponentHealth(int value)
        {
            float sliderValue = (float)_viewModel.OpponentHealth.Value / _viewModel.GetOpponentFullHealth();
            _opponentHealthBar.value = sliderValue;
        }
        private void UpdateOpponentEnergy(int value)
        {
            float sliderValue = (float)_viewModel.OpponentEnergy.Value / _viewModel.GetOpponentFullEnergy();
            _opponentEnergyBar.value = sliderValue;
        }
        
        private void OnDisable()
        {
            /*Cleanup();
            
            _backButton.onClick.RemoveListener(Close);*/
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }


        
        public void Dispose()
        {
            _attackButton.onClick.RemoveListener(_viewModel.PlayerAttack);
            _preparePowerfulAttackButton.onClick.RemoveListener(_viewModel.PlayerPrepareAttack);
            _powerfulAttackButton.onClick.RemoveListener(_viewModel.PlayerPowerfulAttack);
            _blockButton.onClick.RemoveListener(_viewModel.PlayerBlocks);
            _skipTurnButton.onClick.RemoveListener(_viewModel.PlayerSkipTurn);
            _giveUpButton.onClick.RemoveListener(_viewModel.PlayerGiveUp);
            
            _attackOppButton.onClick.RemoveListener(_viewModel.OpponentAttack);
            _preparePowerfulAttackOppButton.onClick.RemoveListener(_viewModel.OpponentPrepareAttack);
            _powerfulAttackOppButton.onClick.RemoveListener(_viewModel.OpponentPowerfulAttack);
            _blockOppButton.onClick.RemoveListener(_viewModel.OpponentBlocks);
            _skipTurnOppButton.onClick.RemoveListener(_viewModel.OpponentSkipTurn);
            _giveUpOppButton.onClick.RemoveListener(_viewModel.OpponentGiveUp);
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
