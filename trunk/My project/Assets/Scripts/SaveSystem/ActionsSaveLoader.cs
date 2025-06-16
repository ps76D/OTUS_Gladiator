using System;
using GameEngine;
using GameEngine.ActionsSystem;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class ActionsSaveLoader : SaveLoader<ActionsService, ActionData>
    {
        override protected ActionData ConvertToData(ActionsService service)
        {
            Debug.Log($"<color=yellow>AvailableActions convert to data = {service.AvailableActions}</color>");
            Debug.Log($"<color=yellow>MaxActionsCount convert to data = {service.MaxActionsCount}</color>");
            return new ActionData
            {
                _availableActions = service.AvailableActions.Value,
                _maxActionsCount = service.MaxActionsCount.Value
            };
        }

        override protected void SetupData(ActionsService service, ActionData data)
        {
            service.SetupAvailableActions(data._availableActions);
            service.SetupMaxActionsCount(data._maxActionsCount);
            Debug.Log($"<color=yellow>Setup AvailableActions = {service.AvailableActions}</color>");
            Debug.Log($"<color=yellow>Setup MaxActionsCount = {service.MaxActionsCount}</color>");
        }

        /*override protected void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }*/
    }
}
