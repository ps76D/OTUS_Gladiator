using GameEngine;
using UniRx;

namespace UI.Model
{
    public interface IBattleModel
    {
        IReadOnlyReactiveProperty<int> PlayerHealth  { get; }
        int GetPlayerFullHealth();
        IReadOnlyReactiveProperty<int> PlayerEnergy  { get; }
        int GetPlayerFullEnergy();
        IReadOnlyReactiveProperty<int> OpponentHealth  { get; }
        int GetOpponentFullHealth();
        IReadOnlyReactiveProperty<int> OpponentEnergy  { get; }
        int GetOpponentFullEnergy();
        
        void PlayerAttack();
        void PlayerPowerfulAttack();
        void PlayerPrepareAttack();
        void PlayerBlocks();
        void PlayerSkipTurn();
        void PlayerGiveUp();
        
        void OpponentAttack();
        void OpponentPowerfulAttack();
        void OpponentPrepareAttack();
        void OpponentBlocks();
        void OpponentSkipTurn();
        void OpponentGiveUp();
        
        BattleService BattleService { get; }
    }
}