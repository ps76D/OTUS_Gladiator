using System;

namespace GameEngine.AI
{
    [Serializable]
    public abstract class BaseNode
    {
        protected readonly BattleService BattleService;

        protected BaseNode(BattleService battleService)
        {
            BattleService = battleService;
        }
    }
}