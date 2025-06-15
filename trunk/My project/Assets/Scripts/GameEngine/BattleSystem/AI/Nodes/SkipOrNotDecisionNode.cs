using UnityEngine;

namespace GameEngine.AI
{
    public class SkipOrNotDecisionNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public SkipOrNotDecisionNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}