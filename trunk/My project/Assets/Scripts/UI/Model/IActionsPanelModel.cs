using System.Collections.Generic;
using UniRx;

namespace UI.Model
{
    public interface IActionsPanelModel
    {
        IReactiveProperty<int> AvailableActionsCount { get; }
        IReactiveProperty<int> MaxActionsCount { get; }
        HashSet<ActionModel> AvailableActions { get; }
        HashSet<ActionModel> UnavailableActions { get; }
        void SpendAction(int actions);
    }
}