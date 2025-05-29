using UniRx;

namespace UI.Model
{
    public interface IHudModel
    {
        IReadOnlyReactiveProperty<int> DayCount { get; }
        IReadOnlyReactiveProperty<int> MoneyCount { get; }
        void EndDay();
        void InGameMenuShow();
    }
}