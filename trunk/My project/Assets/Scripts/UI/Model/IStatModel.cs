using GameEngine.CharacterSystem.StatsSystem;
using UniRx;


namespace UI.Model
{
    public interface IStatModel
    {
        CharacterStat CharacterStat { get; }
        IReadOnlyReactiveProperty<int> StatValue { get; }
        IReadOnlyReactiveProperty<int> CurrentStatExperience { get; }
        IReadOnlyReactiveProperty<int> RequiredStatExperience { get; }
    }
}