using System;

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
        void NewGame();
        IPieceAgent At(Coord coord);
    }
}
