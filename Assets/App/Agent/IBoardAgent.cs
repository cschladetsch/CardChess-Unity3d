using System;
using App.Common.Message;
using Flow;

namespace App.Agent
{
    using Common;
    using Model;

    /// <summary>
    /// An agent that acts on behalf of a model board.
    /// </summary>
    public interface IBoardAgent
        : IAgent<IBoardModel>
    {
        ITransient NewGame();
        IFuture<IPieceAgent> At(Coord coord);
    }
}
