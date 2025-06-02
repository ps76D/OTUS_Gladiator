using UniRx;
using UnityEngine.Localization;

namespace UI.Model
{
    public interface IHudModel
    {
        IReadOnlyReactiveProperty<int> DayCount { get; }
        IReadOnlyReactiveProperty<int> MoneyCount { get; }
        IReadOnlyReactiveProperty<int> LevelCount { get; }
        IReadOnlyReactiveProperty<int> ExpCount { get; }
        IReadOnlyReactiveProperty<int> RequiredExpCount { get; }
        IReadOnlyReactiveProperty<bool> LevelUpButtonIsInteractable { get; }
        IReadOnlyReactiveProperty<bool> ActionsButtonIsInteractable { get; }
        IReadOnlyReactiveProperty<int> CurrentMoral { get; }
        void EndDay();
        void InGameMenuShow();
        void IncreaseStat(string statName);
        void LevelUp();
        void SpendAction(int actions);
        LocalizedString GetMoralState();
        void IncreaseMoral(int moral);
        void DegreaseMoral(int moral);
        void Rest();
    }
}