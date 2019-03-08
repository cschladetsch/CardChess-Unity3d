using Dekuple;
using Dekuple.Model;
using UniRx;

namespace App.Model
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// A card in play as a piece on the board.
    /// </summary>
    public interface IPieceModel
        : IModel
    {
        ICardModel Card { get; }
        EPieceType PieceType { get; }
        EColor Color { get; }
        IReactiveProperty<Coord> Coord { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReadOnlyReactiveProperty<bool> Dead { get; }
        bool AttackedThisTurn { get; set; }
        bool MovedThisTurn { get; set; }

        IResponse Attack(IPieceModel piece);
        IResponse TakeDamage(IPieceModel piece);

        void NewTurn();
    }
}
