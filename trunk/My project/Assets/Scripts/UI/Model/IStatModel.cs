using UniRx;
using UnityEngine;

namespace UI.Model
{
    public interface IStatModel
    {
        IReadOnlyReactiveProperty<int> StatValue { get; }
        Sprite Icon { get; }
    }
}