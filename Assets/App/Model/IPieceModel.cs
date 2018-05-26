using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A card played as a piece on the board
    /// </summary>
    public interface IPieceModel
        : IPlayerOwnedModel
        , IConstructWith<IPlayerModel, ICardModel>
    {
        ICardModel Card { get; }
        EPieceType Type { get; }
        IBoardModel Board { get; }
        Coord Coord { get; }
        int Power { get; }
        int Health { get; }

        void Attack(IPieceModel defender);
    }
}
