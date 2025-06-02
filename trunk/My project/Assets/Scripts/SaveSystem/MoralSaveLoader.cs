using System;
using GameEngine.CharacterSystem;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class MoralSaveLoader : SaveLoader<MoralService, MoralData>
    {
        override protected MoralData ConvertToData(MoralService service)
        {
            Debug.Log($"<color=yellow>AvailableActions convert to data = {service.CurrentMoral}</color>");
            Debug.Log($"<color=yellow>MaxActionsCount convert to data = {service.MaxMoralCount}</color>");
            return new MoralData
            {
                _currentMoral = service.CurrentMoral.Value,
                _maxMoralCount = service.MaxMoralCount.Value
            };
        }

        override protected void SetupData(MoralService service, MoralData data)
        {
            service.SetupCurrentMorals(data._currentMoral);
            service.SetupMaxMoralCount(data._maxMoralCount);
            Debug.Log($"<color=yellow>Setup AvailableActions = {service.CurrentMoral}</color>");
            Debug.Log($"<color=yellow>Setup MaxActionsCount = {service.MaxMoralCount}</color>");
        }

        /*override protected void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }*/
    }
}
