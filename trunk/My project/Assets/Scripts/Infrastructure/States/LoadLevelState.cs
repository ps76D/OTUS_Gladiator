using UI;
using UnityEngine;

namespace Infrastructure
{
  public class LoadLevelState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
    }
    
    public void Enter()
    {
      _sceneLoader.LoadAdditive(SceneNamesConsts.Common, OnLoaded, OnLoadStart);
      
      Debug.Log("Game Enter LoadLevelState");
    }

    public void Exit()
    {
    }

    private void OnLoaded()
    {
      _stateMachine.Enter<MainMenuState>();
    }
    
    private void OnLoadStart()
    {
      _loadingCurtain.Hide();;
    }
  }
}