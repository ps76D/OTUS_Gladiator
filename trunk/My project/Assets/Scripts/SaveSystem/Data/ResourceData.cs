using System;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public struct ResourceData
    {
        [SerializeField]
        public string _id;

        [SerializeField]
        public int _amount;

        public ResourceData(string id, int amount)
        {
            _id = id;
            _amount = amount;
        }
    }
}