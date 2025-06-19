
using UnityEngine;
using UnityEngine.Serialization;

namespace GameEngine
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [Header("Gameplay parameters")]
        [SerializeField] private int _restPrice = 100;
        [SerializeField] private int _restActionRequired = 1;
        [SerializeField] private int _restMoralIncrease = 5;
        [SerializeField] private int _leveExpIncreaseByTraining = 25;
        [SerializeField] private int _statExpIncreaseByTraining = 1;
        
        public int RestPrice => _restPrice;
        public int RestActionRequired => _restActionRequired;
        public int RestMoralIncrease => _restMoralIncrease;
        public int LeveExpIncreaseByTraining => _leveExpIncreaseByTraining;
        public int StatExpIncreaseByTraining => _statExpIncreaseByTraining;


        


    }
}