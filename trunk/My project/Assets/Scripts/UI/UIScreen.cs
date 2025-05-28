using UI.Infrastructure;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        [Inject]
        [SerializeField] protected UIManager _uiManager;
    }
}