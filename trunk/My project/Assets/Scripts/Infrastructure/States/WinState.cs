using System;
using UnityEngine;

namespace Infrastructure
{
  public class WinState : IState
  {
    private readonly GameStateMachine _stateMachine;
    
    public event Action OnWinState;
    
    public WinState(GameStateMachine gameStateMachine)
    {
      _stateMachine = gameStateMachine;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
      OnWinState?.Invoke();
      
      Debug.Log("Enter WinState");
    }
  }
}