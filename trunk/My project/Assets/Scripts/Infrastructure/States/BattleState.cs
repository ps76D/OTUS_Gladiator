using System;
using UnityEngine;

namespace Infrastructure
{
  public class BattleState : IState
  {
    public event Action OnBattleState;

    public BattleState()
    {

    }
    
    public void Exit()
    {
    }

    public void Enter()
    {
      OnBattleState?.Invoke();
      
      Debug.Log("Enter BattleState");
    }
  }
}