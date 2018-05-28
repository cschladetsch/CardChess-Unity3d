using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using Common.Message;

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
        Coord Coord { get; set; }
        int Power { get; }
        int Health { get; }
        bool Dead { get; }
        bool Alive { get; }

        Response Attack(IPieceModel piece);
        Response TakeDamage(IPieceModel piece);

        //Response TakeDamage(IPieceModel self, IPieceModel attacker);
    }
}
