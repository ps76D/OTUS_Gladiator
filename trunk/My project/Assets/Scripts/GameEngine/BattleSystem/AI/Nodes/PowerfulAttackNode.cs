using UnityEngine;

namespace GameEngine.AI
{
    public class PowerfulAttackNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public PowerfulAttackNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter PowerfulAttackNode</color>");
            _battleService.OnOpponentTurnEnd += EnterWaitNode;
            
            _battleService.OpponentPowerfulAttack();
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