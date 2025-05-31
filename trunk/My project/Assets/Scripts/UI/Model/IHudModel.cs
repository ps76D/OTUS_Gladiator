using UniRx;

namespace UI.Model
{
    public interface IHudModel
    {
        IReadOnlyReactiveProperty<int> DayCount { get; }
        IReadOnlyReactiveProperty<int> MoneyCount { get; }
        IReadOnlyReactiveProperty<int> LevelCount { get; }
        IReadOnlyReactiveProperty<int> ExpCount { get; }
        IReadOnlyReactiveProperty<int> RequiredExpCount { get; }
        void EndDay();
        void InGameMenuShow();
    }
}