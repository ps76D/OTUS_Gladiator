using System;
using System.Collections.Generic;

namespace GameEngine.AI
{
  public class BrainStateMachine
  {
    private Dictionary<Type, IExitableBrainState> _states = new();
    private IExitableBrainState _activeState;

    public void Initialize(BattleService battleService)
    {
      _states = new Dictionary<Type, IExitableBrainState>
      {
        [typeof(WaitTurnNode)] = new WaitTurnNode(battleService),
        [typeof(AttackNode)] = new AttackNode(battleService),
        [typeof(BlockNode)] = new BlockNode(battleService),
        [typeof(BlockOrNotDecisionNode)] = new BlockOrNotDecisionNode(battleService),
        [typeof(ChooseAttackTypeDecisionNode)] = new ChooseAttackTypeDecisionNode(battleService),
        [typeof(DeadNode)] = new DeadNode(battleService),
        [typeof(PowerfulAttackNode)] = new PowerfulAttackNode(battleService),
        [typeof(PreparePowerfulAttackNode)] = new PreparePowerfulAttackNode(battleService),
        [typeof(SkipOrNotDecisionNode)] = new SkipOrNotDecisionNode(battleService),
      };
    }
    
    public void Enter<TBrainState>() where TBrainState : class, IBrainState
    {
      IBrainState state = ChangeState<TBrainState>();
      state.Enter();
    }

    private TBrainState ChangeState<TBrainState>() where TBrainState : class, IExitableBrainState
    {
      _activeState?.Exit();
      
      TBrainState state = GetState<TBrainState>();
      _activeState = state;
      
      return state;
    }

    public TBrainState GetState<TBrainState>() where TBrainState : class, IExitableBrainState => 
      _states[typeof(TBrainState)] as TBrainState;
  }
}