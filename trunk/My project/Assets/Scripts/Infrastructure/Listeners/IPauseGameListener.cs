namespace Infrastructure.Listeners
{
    public interface IPauseGameListener : IGameStateListener
    {
        void PauseGame();
    }
}