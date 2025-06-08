using System.Collections;
using System.Collections.Generic;
using GameEngine;
using Infrastructure;
using Sirenix.OdinInspector;
using UI.Infrastructure;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameManager
{
    public class SceneContentSwitcher : MonoBehaviour
    {
        [Inject]
        [SerializeField] private UIManager _uiManager;
        
        [SerializeField] private TrainingSceneContentView _trainingSceneContentView;
        [SerializeField] private BattleSceneView _battleView;
        
        [SerializeField] private List<SceneContentView> _sceneViews = new ();

        private void Start()
        {
            /*_uiManager.MatchmakingView.OnBattleButtonClicked += ShowBattleScene;*/
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<BattleState>().OnBattleState += ShowBattleScene;
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState += ShowTrainingScene;
            /*_uiManager.OnBackToTraining += ShowTrainingScene;*/
        }

        private void OnDisable()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<BattleState>().OnBattleState -= ShowBattleScene;
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnLoadInGameState -= ShowTrainingScene;
        }
        
        public void ShowScene<T>()
        { 
            foreach (SceneContentView sceneView in _sceneViews)
            {
                bool isTarget = sceneView is T;
                sceneView.gameObject.SetActive(isTarget);
            }
        }
        
        [Button]
        public void ShowTrainingScene()
        {
            StartCoroutine(ChangeSceneToTraining<TrainingSceneContentView>());
        }
        
        [Button]
        public void ShowBattleScene()
        {
            StartCoroutine(ChangeSceneToBattle<BattleSceneView>());
        }

        private IEnumerator ChangeSceneToBattle<T>()
        {
            _uiManager.GameBootstrapper.LoadingCurtain.Hide();
            yield return new WaitForSeconds(1f);

            ShowScene<T>();
            
            _uiManager.GameBootstrapper.LoadingCurtain.Show();
        }
        
        private IEnumerator ChangeSceneToTraining<T>()
        {
            _uiManager.GameBootstrapper.LoadingCurtain.Hide();
            yield return new WaitForSeconds(1f);

            ShowScene<T>();
            
            _uiManager.GameBootstrapper.LoadingCurtain.Show();
        }
    }
}