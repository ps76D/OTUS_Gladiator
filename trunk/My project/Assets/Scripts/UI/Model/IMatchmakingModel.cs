using System.Collections.Generic;
using GameEngine.BattleSystem;
using GameEngine.CharacterSystem;

namespace UI.Model
{
    public interface IMatchmakingModel
    {
        List<CharacterInfoSObj> GetCharacters();
        CharacterInfo GetCurrentCharacter();
        void StartMatch();

        MatchMakingService MatchMakingService {
            get;
        }
    }
}