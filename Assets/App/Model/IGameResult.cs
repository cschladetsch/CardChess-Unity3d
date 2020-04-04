namespace App.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: For multi player.
    /// </summary>
    public interface IGameResult
    {
        IList<IContestant> Contestants { get; }
        DateTime Played { get; }
        TimeSpan Duration { get; }
    }
}
