using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonPlaySound : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _soundName = "ButtonClick";

        private void OnEnable()
        {
            _button.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            MasterAudio.PlaySound(_soundName);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlaySound);
        }
    }
}
