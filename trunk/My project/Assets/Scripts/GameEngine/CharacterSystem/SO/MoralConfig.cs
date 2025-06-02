using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.CharacterSystem
{
    [CreateAssetMenu(fileName = "MoralConfig", menuName = "Moral/MoralConfig", order = 0)]
    public class MoralConfig : ScriptableObject
    {
        public List<MoralLevel> _moralLevels;
    }
}