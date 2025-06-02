using System.Collections.Generic;

namespace UI.Model
{
    public interface IStatsModel
    {
        HashSet<StatModel> Stats { get; }
    }
}