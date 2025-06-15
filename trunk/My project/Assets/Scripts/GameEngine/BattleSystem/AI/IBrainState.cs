namespace GameEngine.AI
{
  public interface IBrainState: IExitableBrainState
  {
    void Enter();
    
  }
  public interface IExitableBrainState
  {
    void Exit();
  }
}