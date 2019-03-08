using Dekuple.View;
using UniRx;

namespace App.View
{
    using Agent;
    using Common;

    /// <summary>
    /// View of a piece on the board.
    /// </summary>
    public interface IPieceView
        : IView<IPieceAgent>
    {
        /// <summary>
        /// The location of the piece on the board
        /// </summary>
        IReactiveProperty<Coord> Coord { get; }

        /// <summary>
        /// Is this piece alive?
        /// </summary>
        IReadOnlyReactiveProperty<bool> Dead { get; }

        //void SetAgent(IPlayerView view, IPieceAgent agent);
    }
}
