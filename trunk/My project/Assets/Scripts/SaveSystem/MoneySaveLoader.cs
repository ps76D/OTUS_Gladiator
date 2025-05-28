using System;
using GameEngine;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class MoneySaveLoader : SaveLoader<MoneyStorage, MoneyData>
    {
        override protected MoneyData ConvertToData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Money convert to data = {service.Money}</color>");
            return new MoneyData
            {
                _money = service.Money
            };
        }

        override protected void SetupData(MoneyStorage service, MoneyData data)
        {
            service.SetupMoney(data._money);
            Debug.Log($"<color=yellow>Setup Money data = {service.Money}</color>");
        }

        /*override protected void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }*/
    }
}
