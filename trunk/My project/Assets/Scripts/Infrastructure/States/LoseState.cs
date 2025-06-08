using System;
using UnityEngine;

namespace Infrastructure
{
  public class LoseState : IState
  {
    private readonly GameStateMachine _stateMachine;
    
    public event Action OnLoseState;
    
    public LoseState(GameStateMachine gameStateMachine)
    {
      _stateMachine = gameStateMachine;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
      OnLoseState?.Invoke();
      
      Debug.Log("Enter LoseState");
    }
  }
}