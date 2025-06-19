using System.Collections.Generic;
using AesEncrypt;
using DarkTonic.MasterAudio;
using GameEngine;
using GameEngine.ActionsSystem;
using GameEngine.AI;
using GameEngine.CharacterSystem;
using GameEngine.DaySystem;
using GameEngine.MessagesSystem;
using GameManager;
using Infrastructure.Listeners;
using PlayerProfileSystem;
using SaveSystem;
using UI.Infrastructure;
using UI.SO;
using UnityEngine;
using Zenject;

namespace Infrastructure.DI
{
    public class CommonSystemsInstaller : MonoInstaller
    {
        private List<IGameStateListener> _gameStateListeners;
        [SerializeField] private SaveLoadManager _saveLoadManager;
        /*[SerializeField] private MoneyStorage _moneyStorage;*/
        /*[SerializeField] private DayService _dayService;*/
        
        [SerializeField] private MasterAudio _masterAudio;
        [SerializeField] private SoundSaveLoader _soundSaveLoader;
        [SerializeField] private PlayerProfileDefault _playerProfileDefault;
        [SerializeField] private CharacterInfoSObj _characterInfoDataDefault;
        [SerializeField] private CharacterDatabase _characterDatabase;
        [SerializeField] private MoralConfig _moralConfig;
        [SerializeField] private MatchMakingService _matchMakingService;
        [SerializeField] private BattleService _battleService;
        [SerializeField] private BattleConfig _battleConfig;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private MessagesDatabase _messagesDatabase;
        /*[SerializeField] private CharacterDatabase _enemyDatabase;*/
        
        public override void InstallBindings()
        {
            Container.Bind<MasterAudio>().FromInstance(_masterAudio).AsSingle().NonLazy();
            Container.Bind<SoundSaveLoader>().FromInstance(_soundSaveLoader).AsSingle().NonLazy();
            
            GameBootstrapper gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            UIManager uiManager = FindObjectOfType<UIManager>();

            BindObjectAsSingleNonLazy(gameBootstrapper);
            BindObjectAsSingleNonLazy(uiManager);


            Container.Bind<PlayerProfileDefault>().FromInstance(_playerProfileDefault).AsSingle().NonLazy();
            
            /*Container.Bind<ActionsService>().FromNew().AsSingle().NonLazy();*/
            /*Container.Bind<ISaveLoader>().To<ActionsSaveLoader>().AsCached().NonLazy();*/
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
            Container.Bind<MoralConfig>().FromInstance(_moralConfig).AsSingle().NonLazy();

            Container.Bind<MoralService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<MoralSaveLoader>().AsCached().NonLazy();

            Container.Bind<MoneyStorage>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<MoneySaveLoader>().AsCached().NonLazy();

            Container.Bind<DayService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<DaySaveLoader>().AsCached().NonLazy();

            Container.Bind<CharacterInfoSObj>().FromInstance(_characterInfoDataDefault).AsSingle().NonLazy();
            Container.Bind<CharacterDatabase>().FromInstance(_characterDatabase).AsSingle().NonLazy();

            Container.Bind<MessagesDatabase>().FromInstance(_messagesDatabase).AsSingle().NonLazy();
            /*Container.Bind<CharacterDatabase>().FromInstance(_enemyDatabase).AsSingle().NonLazy();*/

            Container.Bind<CharacterService>().ToSelf().AsSingle().NonLazy();
            Container.Bind<ISaveLoader>().To<CharacterSaveLoader>().AsCached().NonLazy();


            Container.Bind<PlayerProfile>().ToSelf().AsSingle().NonLazy();

            Container.Bind<ProfileService>().ToSelf().AsSingle().NonLazy();
            
            Container.Bind<MatchMakingService>().FromInstance(_matchMakingService).AsSingle().NonLazy();
            Container.Bind<BattleConfig>().FromInstance(_battleConfig).AsSingle().NonLazy();
            Container.Bind<BrainStateMachine>().FromNew().AsSingle().NonLazy();
            Container.Bind<BattleService>().FromInstance(_battleService).AsSingle().NonLazy();

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