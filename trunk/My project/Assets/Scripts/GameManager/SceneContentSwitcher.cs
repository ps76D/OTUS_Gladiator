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
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnGameLoopSceneLoaded += ShowTrainingScene;
            /*_uiManager.OnBackToTraining += ShowTrainingScene;*/
        }

        private void OnDisable()
        {
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<BattleState>().OnBattleState -= ShowBattleScene;
            _uiManager.GameBootstrapper.Game.StateMachine.GetState<LoadInGameState>().OnGameLoopSceneLoaded -= ShowTrainingScene;
        }
        
        public void ShowScene(SceneContentView sceneToLoad)
        { 
            foreach (SceneContentView sceneView in _sceneViews)
            {

                sceneView.gameObject.SetActive(false);
            }
            
            sceneToLoad.gameObject.SetActive(true);
            
        }
        
        [Button]
        public void ShowTrainingScene()
        {
            StartCoroutine(ChangeSceneToTraining(_trainingSceneContentView));
        }
        
        [Button]
        public void ShowBattleScene()
        {
            StartCoroutine(ChangeSceneToBattle(_battleView));

        }

        private IEnumerator ChangeSceneToBattle<T>(T sceneToLoad) where T : SceneContentView
        {
            _uiManager.GameBootstrapper.LoadingCurtain.Hide();
            yield return new WaitForSeconds(1f);

            ShowScene(sceneToLoad);
            
            _uiManager.GameBootstrapper.LoadingCurtain.Show();
        }
        
        private IEnumerator ChangeSceneToTraining<T>(T sceneToLoad) where T : SceneContentView
        {
            /*_uiManager.GameBootstrapper.LoadingCurtain.Hide();
            yield return new WaitForSeconds(1f);*/
            yield return null;
            
            ShowScene(sceneToLoad);

            _uiManager.GameBootstrapper.LoadingCurtain.Show();
        }
    }
}