using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine
{
    [Serializable]
    public sealed class MoneyStorage
    {
        public event Action<int> OnMoneyChanged;

        public int Money => _money;

        [NaughtyAttributes.ReadOnly]
        [ShowInInspector]
        private int _money;

        [Button]
        public void SetupMoney(int money)
        {
            _money = money;
            OnMoneyChanged?.Invoke(_money);
        }

        [Button]
        public void AddMoney(int range)
        {
            _money += range;
            OnMoneyChanged?.Invoke(_money);
        }

        [Button]
        public void SpendMoney(int range)
        {
            _money -= range;
            OnMoneyChanged?.Invoke(_money);
        }
    }
}