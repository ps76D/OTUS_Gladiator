namespace Infrastructure.Listeners
{
    public interface IInGameListener : IGameStateListener
    {
        void InGame();
    }
}