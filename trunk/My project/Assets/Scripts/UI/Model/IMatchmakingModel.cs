using System.Collections.Generic;
using GameEngine;
using GameEngine.CharacterSystem;
using GameManager;
using UniRx;

namespace UI.Model
{
    public interface IMatchmakingModel
    {
        IReadOnlyReactiveProperty<bool> IsOpponentSelected { get; }
        List<CharacterInfoSObj> GetCharacters();
        CharacterInfo GetCurrentCharacter();
        void StartMatch();

        MatchMakingService MatchMakingService {
            get;
        }
    }
}