using System;
using GameEngine;
using GameEngine.DaySystem;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class DaySaveLoader : SaveLoader<DayService, DayData>
    {
        override protected DayData ConvertToData(DayService service)
        {
            Debug.Log($"<color=yellow>Day convert to data = {service.Day}</color>");
            return new DayData
            {
                _day = service.Day
            };
        }

        override protected void SetupData(DayService service, DayData data)
        {
            service.SetupDay(data._day);
            Debug.Log($"<color=yellow>Setup Day data = {service.Day}</color>");
        }

        /*override protected void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }*/
    }
}
