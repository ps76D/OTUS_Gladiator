namespace Infrastructure.Listeners
{
    public interface IFinishGameListener : IGameStateListener
    {
        void FinishGame();
    }
}