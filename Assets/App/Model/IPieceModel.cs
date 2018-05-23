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
        int Damage { get; }
        int Health { get; }
        bool Alive { get; }

        IEnumerable<IPieceModel> Attacking();
        IEnumerable<IPieceModel> Defending();
        IEnumerable<IPieceModel> Attackers();
        IEnumerable<IPieceModel> Defenders();
        IEnumerable<IPieceModel> Neareby(int distance);

        void Respond(IPieceModel attacker);
        void Attack(IPieceModel defender);
    }
}
