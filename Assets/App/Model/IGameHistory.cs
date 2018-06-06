using System;
using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// History of games played by the a given user.
    /// </summary>
    public interface IGameHistory
    {
        Guid User { get; }
        IEnumerable<IGameResult> GameResults { get; }
    }
}
