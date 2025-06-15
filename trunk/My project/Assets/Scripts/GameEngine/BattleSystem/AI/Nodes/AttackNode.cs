using UnityEngine;

namespace GameEngine.AI
{
    public class AttackNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public AttackNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter AttackNode</color>");
            _battleService.OnOpponentTurnEnd += EnterWaitNode;
            
            _battleService.OpponentAttack();
        }

        public void Exit()
        {
            _battleService.OnOpponentTurnEnd -= EnterWaitNode;
        }

        private void EnterWaitNode()
        {
            _battleService.BrainStateMachine.Enter<WaitTurnNode>();
        }
    }
}