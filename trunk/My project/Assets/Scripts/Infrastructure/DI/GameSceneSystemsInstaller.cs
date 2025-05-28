using System;
using System.Collections.Generic;
using System.Linq;
// using Bullets;
// using Components;
// using Enemy;
using Infrastructure.CommonInterfaces;
using Infrastructure.Listeners;
/*using Input;
using Level;*/
using UI.Infrastructure;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
/*using CharacterController = Character.CharacterController;*/

namespace Infrastructure.DI
{
    public class GameSceneSystemsInstaller : MonoInstaller
    {
        [SerializeField] private UpdateController _updateController;
        
        /*[SerializeField] private InputConfig _inputConfig;
        [SerializeField] private BackgroundConfig _backgroundConfig;

        [SerializeField] private HitPointsComponent _character;

        [SerializeField] private LevelBounds _levelBounds;
        
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private BulletPool _bulletPool;

        [SerializeField] private Transform _worldTransform;*/
        
        /*[SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private EnemyPositionsProvider _enemyPositionsProvider;*/
        
        private IGameStateListener[] _gameStateListeners;
        
        public override void InstallBindings()
        {
            /*Container.BindInterfacesAndSelfTo<InputManager>().FromNew().AsSingle().WithArguments(_inputConfig).NonLazy();

            Container.Bind<UpdateController>().FromInstance(_updateController).AsCached().NonLazy();

            Container.Bind<EnemyManager>().ToSelf().AsSingle().WithArguments(_enemyPool, _enemyPrefab, _worldTransform, _enemyPositionsProvider, _character.transform);

            Container.Bind<BulletManager>().ToSelf().AsSingle().WithArguments(_bulletPrefab, _bulletPool, _worldTransform, _updateController);

            Container.Bind<CharacterController>().FromNew().AsSingle().WithArguments(_character).NonLazy();
            
            BindObject(_levelBounds);
            
            Container.BindInterfacesAndSelfTo<LevelBackgroundMover>().FromNew().AsSingle().WithArguments(_backgroundConfig, _levelBounds).NonLazy();*/
            
 
            Container.BindInterfacesAndSelfTo<IFixedUpdatable>().FromComponentsInHierarchy().AsCached().NonLazy();
            
            var gameStateListeners = FindObjectsOfType<MonoBehaviour>(true).OfType<IGameStateListener>().ToList();
            Container.Bind<IEnumerable<IGameStateListener>>().FromInstance(gameStateListeners).AsCached().NonLazy();
        }

        private void BindObject<T>(T obj)
        {
            if (obj != null)
            {
                Container.Bind<T>()
                    .FromInstance(obj)
                    .AsSingle().NonLazy();
            }
        }
    }
}