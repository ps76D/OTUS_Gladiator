using CodeBase.Infrastructure;
using UI;
using UI.Infrastructure;
using UnityEngine;

namespace Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    [SerializeField] private LoadingCurtain _loadingCurtain;
    [SerializeField] private Camera _commonCamera;
    
    public Game Game;

    public Camera CommonCamera => _commonCamera;
    public LoadingCurtain LoadingCurtain => _loadingCurtain;

    private void Awake()
    {
      Game = new Game(this, _loadingCurtain);
      Game.StateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}