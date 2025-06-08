using System;
using System.Collections;
using GameManager;
using UI;
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