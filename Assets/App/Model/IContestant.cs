namespace App.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: for multi player.
    /// </summary>
    public interface IContestant
    {
        Guid UserId { get; }
        IList<Guid> StartingDeck { get; }
        EGameResult Result { get; }
        DateTime Played { get; }
    }
}
