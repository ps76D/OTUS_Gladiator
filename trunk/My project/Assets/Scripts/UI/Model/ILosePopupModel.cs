using UniRx;

namespace UI.Model
{
    public interface ILosePopupModel
    {
        IReadOnlyReactiveProperty<string> MoralLevelChanged  { get; }
        void BackToTraining();
    }
}