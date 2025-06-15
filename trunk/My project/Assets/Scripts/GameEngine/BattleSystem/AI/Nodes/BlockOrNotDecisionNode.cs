using UnityEngine;

namespace GameEngine.AI
{
    public class BlockOrNotDecisionNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;

        /*public new bool IsNeedToUpdateOnEnter = true;*/
        public BlockOrNotDecisionNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter BlockOrNotDecisionNode</color>");
            
            _battleService.OnOpponentDecideToBlock += EnterBlockNode;
            _battleService.OnOpponentDecideToNotBlock += EnterChooseAttackNode;
            
            //TODO шанс вынести в конфиг
            _battleService.ThrowDoBlocking(60);
        }

        public void Exit()
        {
            _battleService.OnOpponentDecideToBlock -= EnterBlockNode;
            _battleService.OnOpponentDecideToNotBlock -= EnterChooseAttackNode;
        }
        
        public void EnterBlockNode()
        {
            _battleService.BrainStateMachine.Enter<BlockNode>();
        }
        
        public void EnterChooseAttackNode()
        {
            _battleService.BrainStateMachine.Enter<ChooseAttackTypeDecisionNode>();
        }
    }
}