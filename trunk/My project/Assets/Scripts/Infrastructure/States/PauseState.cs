using System;
using UnityEngine;

namespace Infrastructure
{
  public class PauseState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    public event Action OnPauseState;
    
    public PauseState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
      OnPauseState?.Invoke();
 
      Debug.Log("Game Enter PauseState");
    }


  }
}