﻿using System;
using System.Collections;
using GameManager;
using UI;
using UnityEngine;

namespace Infrastructure
{
  public class GameLoopState : IState
  {
    public event Action OnGameLoopState;

    public GameLoopState()
    {

    }
    
    public void Exit()
    {
    }

    public void Enter()
    {
      OnGameLoopState?.Invoke();
      
      Debug.Log("Enter GameLoopState");
    }
  }
}