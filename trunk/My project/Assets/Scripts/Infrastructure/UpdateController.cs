using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.CommonInterfaces;
using Infrastructure.DI;
using Infrastructure.Listeners;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class UpdateController : MonoBehaviour, IPauseGameListener, IFinishGameListener, IResumeGameListener, IInGameListener
    {
        [SerializeField] private float _duration = 3f;
        [SerializeField] private float _startOffset;
        
        [Inject]
        private IEnumerable<IUpdatable> _updatable;
        
        [Inject]
        private IEnumerable<IFixedUpdatable> _fixedUpdatable;

        private GameStateMachine _gameStateMachine;

        [SerializeField] private bool _isNeedUpdate;

        public UpdateController()
        {
            PoolFixedUpdatable = new List<IFixedUpdatable>();
        }

        public List<IFixedUpdatable> PoolFixedUpdatable {
            get;
        }
        
        private void Update()
        {
            if (!_isNeedUpdate) return;
            foreach (IUpdatable updatable in _updatable)
            {
                updatable.CustomUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (!_isNeedUpdate) return;
            foreach (IFixedUpdatable fixedUpdatable in _fixedUpdatable)
            {
                fixedUpdatable.CustomFixedUpdate();
            }

            foreach (var item in PoolFixedUpdatable)
            {
                item.CustomFixedUpdate();
            }
        }
        
        public void PauseGame()
        {
            _isNeedUpdate = false;
        }

        public void FinishGame()
        {
            _isNeedUpdate = false;
        }

        public void ResumeGame()
        {
            _isNeedUpdate = false;
        }

        public void InGame()
        {
            Debug.Log("InGame in Update Controller");
            _isNeedUpdate = false;
            StartCoroutine(StartUpdateCoroutine());
        }
        
        private IEnumerator StartUpdateCoroutine()
        {
            yield return new WaitForSeconds(_duration + _startOffset);
            
            _isNeedUpdate = true;
        }
    }
}