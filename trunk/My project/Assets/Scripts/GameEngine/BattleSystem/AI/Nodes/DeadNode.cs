using UnityEngine;

namespace GameEngine.AI
{
    public class DeadNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public DeadNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter DeadNode</color>");
        }

        public void Exit()
        {
            
        }
    }
}