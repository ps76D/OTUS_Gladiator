using UniRx;

namespace UI.Model
{
    public interface IWinPopupModel
    {
        IReadOnlyReactiveProperty<int> RewardCount  { get; }
        IReadOnlyReactiveProperty<string> MoralLevelChanged  { get; }
        void BackToTraining();
    }
}