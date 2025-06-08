using UnityEngine;
using Zenject;

namespace Infrastructure.DI
{
    public class SceneInjector : MonoBehaviour
    { 
        [Inject] private DiContainer _container;
        
        private void Awake()
        {
            Inject();
        }

        private void Inject()
        {
            /*var manager = FindObjectOfType<HUDScreen>(true);*/
            
            /*if (manager != null)
            {
                _container.Inject(manager);
            }*/
        }
    }
}