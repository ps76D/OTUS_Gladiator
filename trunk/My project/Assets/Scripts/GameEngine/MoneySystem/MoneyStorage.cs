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

        [ShowInInspector, ReadOnly]
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
            if (_money >= range)
            {            
                _money -= range;
                OnMoneyChanged?.Invoke(_money);
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}