using System;
using GameManager;
using UI;
using UI.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
  public class LoadSavedGameState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    public event Action OnLoadSavedGameState;
    public event Action OnGameLoopSceneLoaded;
 
    public LoadSavedGameState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
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
      _sceneLoader.UnLoadThenLoadSceneAdditive(SceneNamesConsts.Game, OnLoaded, OnLoadStart);

      /*_sceneLoader.UnloadIfSceneLoaded(SceneNamesConsts.MainMenu);*/
      
      /*_sceneLoader.ReLoadAdditive(SceneNamesConsts.Game, OnLoaded);*/


      OnLoadSavedGameState?.Invoke();

      Debug.Log("Enter LoadSavedGameState");
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