namespace Infrastructure.Listeners
{
    public interface IResumeGameListener : IGameStateListener
    {
        void ResumeGame();
    }
}