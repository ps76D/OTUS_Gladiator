using UnityEngine;

namespace GameEngine.AI
{
    public class ChooseAttackTypeDecisionNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public ChooseAttackTypeDecisionNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }
        
        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter ChooseAttackTypeDecisionNode</color>");
            _battleService.OnOpponentDecideToPowerfulAttack += EnterPreparePowerfulAttackNode;
            _battleService.OnOpponentDecideToAttack += EnterAttackNode;
            
            _battleService.ThrowChooseAttackType(70);
        }

        public void Exit()
        {
            _battleService.OnOpponentDecideToPowerfulAttack -= EnterPreparePowerfulAttackNode;
            _battleService.OnOpponentDecideToAttack -= EnterAttackNode;
        }

        private void EnterPreparePowerfulAttackNode()
        {
            _battleService.BrainStateMachine.Enter<PreparePowerfulAttackNode>();
        }

        private void EnterAttackNode()
        {
            _battleService.BrainStateMachine.Enter<AttackNode>();
        }
    }
}