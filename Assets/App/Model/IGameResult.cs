using System;
using System.Collections.Generic;

namespace App.Model
{
    public interface IGameResult
    {
        IList<IContestant> Contestants { get; }
        DateTime Played { get; }
        TimeSpan Duration { get; }
    }
}
