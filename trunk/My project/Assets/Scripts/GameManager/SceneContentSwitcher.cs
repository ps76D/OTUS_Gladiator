using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI;
using UI.Infrastructure;
using UnityEngine;
using Zenject;

namespace GameManager
{
    public class SceneContentSwitcher : MonoBehaviour
    {
        [Inject]
        [SerializeField] private UIManager _uiManager;
        
        [SerializeField] private TrainingSceneView _trainingSceneView;
        [SerializeField] private BattleSceneView _battleView;
        
        [SerializeField] private List<SceneView> _sceneViews = new ();

        private void Start()
        {
            _uiManager.MatchmakingView.OnBattleButtonClicked += ShowBattleScene;
        }

        public void ShowScene<T>() where T : MonoBehaviour
        { 
            foreach (SceneView sceneView in _sceneViews)
            {
                bool isTarget = sceneView is T;
                sceneView.gameObject.SetActive(isTarget);
            }
        }
        
        [Button]
        public void ShowTrainingScene()
        {
            StartCoroutine(ChangeSceneToBattle<TrainingSceneView>());
        }
        
        [Button]
        public void ShowBattleScene()
        {
            StartCoroutine(ChangeSceneToBattle<BattleSceneView>());
        }

        private IEnumerator ChangeSceneToBattle<T>() where T : MonoBehaviour
        {
            _uiManager.GameBootstrapper.LoadingCurtain.Hide();
            yield return new WaitForSeconds(1f);

            ShowScene<T>();
            
            _uiManager.GameBootstrapper.LoadingCurtain.Show();
        }
    }
}