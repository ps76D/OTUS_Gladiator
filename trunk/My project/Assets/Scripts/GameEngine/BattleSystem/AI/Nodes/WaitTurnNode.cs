using UnityEngine;

namespace GameEngine.AI
{
    public class WaitTurnNode : BaseNode, IBrainState
    {
        private readonly BattleService _battleService;
        
        public WaitTurnNode(BattleService battleService ) : base(battleService)
        {
            _battleService = battleService;
        }

        public void Enter()
        {
            Debug.Log("<color=orange>Opponent Enter WaitTurnNode</color>");

            _battleService.OnOpponentTurn += ChooseWhatToDo;
            _battleService.OnOpponentDead += EnterDeadNode;
        }

        public void Exit()
        {
            _battleService.OnOpponentTurn -= ChooseWhatToDo;
            _battleService.OnOpponentDead -= EnterDeadNode;
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