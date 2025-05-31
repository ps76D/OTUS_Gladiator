using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UI.Model
{
    public interface IStatsModel
    {
        HashSet<StatModel> Stats { get; }
    }
}