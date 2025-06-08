using System;
using UI;
using UnityEngine;

namespace Infrastructure
{
  public class LoadInGameState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    public event Action OnLoadInGameState;
    public event Action OnGameLoopSceneLoaded;
 
    public LoadInGameState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
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
      /*_sceneLoader.UnloadIfSceneLoaded(SceneNamesConsts.Game);*/
      
      _sceneLoader.ReLoadAdditive(SceneNamesConsts.Game, OnLoaded, OnLoadStart);
      
      _sceneLoader.UnloadIfSceneLoaded(SceneNamesConsts.MainMenu);
      
      OnLoadInGameState?.Invoke();

      Debug.Log("Enter LoadInGameState");
    }
    
    private void OnLoaded()
    {
      OnGameLoopSceneLoaded?.Invoke();
      
      _loadingCurtain.Show();
      
      _stateMachine.Enter<GameLoopState>();
      
    }
    
    private void OnLoadStart()
    {
      _loadingCurtain.Hide();
    }
  }
}