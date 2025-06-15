using UnityEngine;

namespace GameEngine.AI
{
    public class BlockNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public BlockNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter BlockNode</color>");

            _battleService.OnOpponentBlocked += RealiseBlock;
            _battleService.OnOpponentTurn += ChooseWhatToDo;
            _battleService.OnOpponentDead += EnterDeadNode;
            
            _battleService.OpponentBlocks();
        }

        public void Exit()
        {
            _battleService.OnOpponentBlocked -= RealiseBlock;
            _battleService.OnOpponentTurn -= ChooseWhatToDo;
            _battleService.OnOpponentDead -= EnterDeadNode;
        }

        private void RealiseBlock()
        {
            _battleService.BrainStateMachine.Enter<WaitTurnNode>();
        }

        private void EnterDeadNode()
        {
            _battleService.BrainStateMachine.Enter<DeadNode>();
        }

        private void ChooseWhatToDo()
        {
            if (_battleService.IsOpponentPowerfulAttackPrepared)
            {
                _battleService.BrainStateMachine.Enter<PowerfulAttackNode>();
            }
            else if (_battleService.IsPlayerPowerfulAttackPrepared.Value)
            {
                _battleService.BrainStateMachine.Enter<BlockOrNotDecisionNode>();
            }
            else
            {
                _battleService.BrainStateMachine.Enter<ChooseAttackTypeDecisionNode>();
            }
        }
    }
}