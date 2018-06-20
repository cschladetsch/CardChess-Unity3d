using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Common.Message;
    using Model;

    /// <summary>
    /// An agent that acts on behalf of a model board.
    /// </summary>
    public interface IBoardAgent
        : IAgent<IBoardModel>
        , IPrintable
    {
        IReadOnlyReactiveProperty<int> Width { get; }
        IReadOnlyReactiveProperty<int> Height { get; }
        IReadOnlyReactiveCollection<IPieceAgent> Pieces { get; }

        ITransient PerformNewGame();
        IPieceAgent At(Coord coord);

        IResponse Add(IPieceAgent agent);
        IResponse Move(IPieceAgent agent, Coord coord);
        IResponse Remove(IPieceAgent agent);
    }
}
