using UnityEngine;

namespace GameEngine.AI
{
    public class PreparePowerfulAttackNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public PreparePowerfulAttackNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter PreparePowerfulAttackNode</color>");
            _battleService.OnOpponentTurnEnd += EnterWaitNode;
            
            _battleService.OpponentPreparePowerfulAttack();
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