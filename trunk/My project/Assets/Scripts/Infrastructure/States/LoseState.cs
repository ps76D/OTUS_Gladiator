using System;
using GameManager;
using UI;
using UnityEngine;

namespace Infrastructure
{
  public class LoseState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    
    public event Action OnLoseState;
    
    public LoseState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
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