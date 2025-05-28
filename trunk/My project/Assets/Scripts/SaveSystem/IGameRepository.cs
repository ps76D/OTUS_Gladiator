namespace SaveSystem
{
    public interface IGameRepository
    {
        bool TryGetData<T>(out T data);
        void SetData<T>(T data);
    }
}