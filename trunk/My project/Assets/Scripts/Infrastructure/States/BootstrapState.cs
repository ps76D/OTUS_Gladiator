using UnityEngine;

namespace Infrastructure
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
      /*this.RegisterServices();*/
      _sceneLoader.Load(SceneNamesConsts.Initial, onLoaded: EnterLoadLevel);
      Debug.Log("Game Enter BootstrapState");
    }

    public void Exit()
    {
    }

    private void EnterLoadLevel() => 
      _stateMachine.Enter<LoadLevelState>();

    /*private void RegisterServices()
    {

    }*/
  }
}