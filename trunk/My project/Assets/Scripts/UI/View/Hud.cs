using System;
using TMPro;
using UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Hud : UIScreen
    {
        [SerializeField] private Button _endDayButton;
        [SerializeField] private TMP_Text _currentDayText;
        
        /*[SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _settingsButton;*/
        
        private HudModel _hudModel;

        private void OnEnable()
        {
            _hudModel = new HudModel(_uiManager, _uiManager.ProfileService.PlayerProfile.DayService);
            
            _hudModel.DayCount.Subscribe(_ => { UpdateDayText(); });
            
            _endDayButton.onClick.AddListener(EndDayButton);
            
            /*_loadGameButton.onClick.AddListener(LoadGameButton);
            _saveGameButton.onClick.AddListener(StartGameButton);
            _settingsButton.onClick.AddListener(StartGameButton);*/
        }

        private void EndDayButton()
        {
            EndDay(_hudModel);
        }
        
        private void EndDay(HudModel model)
        {
            model.OnEndDayButtonClicked?.Invoke();
        }

        private void UpdateDayText()
        {
            _currentDayText.text = _hudModel.DayCount.Value.ToString();

        }
    }
}