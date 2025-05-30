using System.Collections.Generic;
using System.Linq;
using AesEncrypt;
using Character;
using GameEngine;
using GameEngine.CharacterSystem;
using Infrastructure.Listeners;
using PlayerProfileSystem;
using SaveSystem;
using UI.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.DI
{
    public class CommonSystemsInstaller : MonoInstaller
    {
        private List<IGameStateListener> _gameStateListeners;
        [SerializeField] private SaveLoadManager _saveLoadManager;
        /*[SerializeField] private MoneyStorage _moneyStorage;*/
        /*[SerializeField] private DayService _dayService;*/
        
        [SerializeField] private PlayerProfileDefault _playerProfileDefault;
        [SerializeField] private CharacterInfoSObj _characterInfoDataDefault;
        [SerializeField] private CharacterDatabase _characterDatabase;
        
        public override void InstallBindings()
        {
            GameBootstrapper gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            UIManager uiManager = FindObjectOfType<UIManager>();

            BindObjectAsSingleNonLazy(gameBootstrapper);
            BindObjectAsSingleNonLazy(uiManager);

            Container.Bind<ActionsService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<ActionsSaveLoader>().AsCached().NonLazy();
            
            Container.Bind<MoneyStorage>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<MoneySaveLoader>().AsCached().NonLazy();

            Container.Bind<DayService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<DaySaveLoader>().AsCached().NonLazy();

            Container.Bind<CharacterInfoSObj>().FromInstance(_characterInfoDataDefault).AsSingle().NonLazy();
            Container.Bind<CharacterDatabase>().FromInstance(_characterDatabase).AsSingle().NonLazy();

            Container.Bind<CharacterService>().ToSelf().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<CharacterSaveLoader>().AsCached().NonLazy();
            
            Container.Bind<PlayerProfileDefault>().FromInstance(_playerProfileDefault).AsSingle().NonLazy();

            Container.Bind<PlayerProfile>().ToSelf().AsSingle().NonLazy();

            Container.Bind<ProfileService>().ToSelf().AsSingle().NonLazy();


            Container.Bind<AesEncryptComponent>().FromNew().AsSingle().NonLazy();
            Container.Bind<GameRepository>().FromNew().AsSingle().NonLazy();

            Container.Bind<SaveLoadManager>().FromInstance(_saveLoadManager).AsCached().NonLazy();
            

        }

        private void BindObjectAsSingleNonLazy<T>(T obj)
        {
            if (obj != null)
            {
                Container.Bind<T>().FromInstance(obj).AsSingle().NonLazy();
            }
            else
            {
                Debug.LogError( "Binding Object not found");
            } 
        }
    }
}