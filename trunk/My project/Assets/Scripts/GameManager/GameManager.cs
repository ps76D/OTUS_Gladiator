using System;
using System.Collections.Generic;
/*using Components;*/
using Infrastructure;
using Infrastructure.DI;
/*using Input;*/
using UnityEngine;
using Zenject;
/*using CharacterController = Character.CharacterController;*/

namespace GameManager
{
    public sealed class GameManager : MonoBehaviour
    {
        [Inject]
        [SerializeField] private GameBootstrapper _gameBootstrapper;
        
        [Inject]
        private CharacterController _characterController;
        
        /*[Inject]
        private InputManager _inputManager;*/

        private GameStateMachine _gameStateMachine;

        private void Start()
        {
            _gameStateMachine = _gameBootstrapper.Game.StateMachine;
            
            /*_gameStateMachine.GetState<GameLoopState>().OnGameLoopState += Revive;*/
            
            /*_characterController.OnCharacterDeath += FinishGame;*/
            
            Debug.Log("GameManager Started");
        }

        private void OnDisable()
        {
            /*_gameStateMachine.GetState<GameLoopState>().OnGameLoopState -= Revive;*/
            
            /*_characterController.OnCharacterDeath -= FinishGame;*/
        }

        private void Revive()
        {
            // HitPointsComponent character = _characterController.Character;

            /*
            character.TurnOnOffCollider(true);
            
            character.Revive();*/
            
            EnablePlayerInput(true);
        }

        private void FinishGame()
        {
            _gameStateMachine.Enter<LoseState>();

            // HitPointsComponent character = _characterController.Character;
            // character.TurnOnOffCollider(false);
            
            Debug.Log("Game over!");
            
            EnablePlayerInput(false);
        }

        private void EnablePlayerInput(bool value)
        {
            /*_inputManager.IsActive = value;*/
        }
    }
}