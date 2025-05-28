using UnityEngine;

namespace SaveSystem
{
    public abstract class SaveLoader<TService, TData> : ISaveLoader
    {

        public void SaveGame(SaveLoadManager saveLoadManager, IGameRepository gameRepository)
        {
            TService service = saveLoadManager.GetService<TService>();
            TData data = ConvertToData(service);
            gameRepository.SetData(data);

            Debug.Log($"Saved data {data.GetType().Name}");
        }

        public void LoadGame(SaveLoadManager saveLoadManager, IGameRepository gameRepository)
        {
            TService service = saveLoadManager.GetService<TService>();
            
            if (gameRepository.TryGetData(out TData data))
            {
                SetupData(service, data);
                Debug.Log($"{data.GetType().Name} loaded");
            }
            else
            {
                SetupDefaultData(service);
                Debug.Log($"{data.GetType().Name} not loaded");
            }
        }

        protected abstract TData ConvertToData(TService service);
        protected abstract void SetupData(TService service, TData data);

        protected virtual void SetupDefaultData(TService service) { }
    }
}