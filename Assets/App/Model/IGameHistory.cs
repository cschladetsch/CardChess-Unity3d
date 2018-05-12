using System;
using System.Collections.Generic;

namespace App.Model
{
    public interface IGameHistory
    {
        Guid User { get; }
        IEnumerable<IGameResult> GameResults { get; }
    }
}
