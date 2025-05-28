namespace SaveSystem
{
    public interface ISaveLoader
    {
        void SaveGame(SaveLoadManager saveLoadManager, IGameRepository gameRepository);
        void LoadGame(SaveLoadManager saveLoadManager, IGameRepository gameRepository);
    }
}