using GameEngine.CharacterSystem;
using GameManager;

namespace UI.Model
{
    public interface IUnitModel
    {
        MatchMakingService MatchMakingService { get; }
        CharacterInfoSObj CharacterInfoSObj { get; }
        void SelectOpponent();
    }
}