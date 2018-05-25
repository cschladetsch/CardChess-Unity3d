using System;
using System.Collections.Generic;

namespace App.Model
{
    public interface IContestant
    {
        Guid UserId { get; }
        IList<Guid> StartingDeck { get; }
        EGameResult Result { get; }
    }

    public interface IGameResult
    {
        IList<IContestant> Contestants { get; }
        DateTime Played { get; }
        TimeSpan Duration { get; }
    }
}
