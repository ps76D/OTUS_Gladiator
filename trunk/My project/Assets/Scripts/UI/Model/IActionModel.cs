using UniRx;

namespace UI.Model
{
    public interface IActionModel
    {
        IReadOnlyReactiveProperty<bool> ActionAvailable { get; }
    }
}