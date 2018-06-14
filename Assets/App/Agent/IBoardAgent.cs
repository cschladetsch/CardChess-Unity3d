using App.Common.Message;
using App.View;
using Flow;
using UniRx;

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
        IReadOnlyReactiveProperty<int> Width { get; }
        IReadOnlyReactiveProperty<int> Height { get; }

        ITransient PerformNewGame();

        IPieceAgent At(Coord coord);
        IResponse Add(IPieceAgent agent);
        IResponse Remove(IPieceAgent agent);
    }
}
