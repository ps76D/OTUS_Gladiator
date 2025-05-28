using Infrastructure.DI;
using Infrastructure.Listeners;
using UnityEngine;
using Zenject;

namespace UI.Infrastructure
{
    public abstract class UIScreen : MonoBehaviour, IGameStateListener, IUIScreen
    {
        [Inject]
        [SerializeField] protected UIManager _uiManager;
    }
}