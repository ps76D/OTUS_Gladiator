﻿using System;
using UI;
using UnityEngine;

namespace Infrastructure
{
  public class MainMenuState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    
    public event Action OnMainMenuState;
    public event Action OnMainMenuSceneLoaded;

    public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
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
      _sceneLoader.ReLoadAdditive(SceneNamesConsts.MainMenu, OnLoaded, OnLoadStart);

      _sceneLoader.UnloadIfSceneLoaded(SceneNamesConsts.Game);

      OnMainMenuState?.Invoke();
      
      Debug.Log("Game Enter MainMenuState");
    }
    
    private void OnLoaded()
    {
      _loadingCurtain.Show();
      
      OnMainMenuSceneLoaded?.Invoke();
    }
    
    private void OnLoadStart()
    {
      _loadingCurtain.Hide();;
    }
  }
}