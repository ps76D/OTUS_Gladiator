using System.Collections.Generic;
using GameEngine;
using GameEngine.BattleSystem;
using GameEngine.CharacterSystem;
using GameManager;

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