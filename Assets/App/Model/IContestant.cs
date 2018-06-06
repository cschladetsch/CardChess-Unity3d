using System;
using System.Collections.Generic;

namespace App.Model
{
    public interface IContestant
    {
        Guid UserId { get; }
        IList<Guid> StartingDeck { get; }
        EGameResult Result { get; }
        DateTime Played { get; }
    }
}
