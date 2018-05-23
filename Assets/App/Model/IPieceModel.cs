using System.Collections.Generic;
using Flow;

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

        IGenerator Battle(ICardModel other);

        IEnumerable<IPieceModel> Attacking();
        IEnumerable<IPieceModel> Defending();
        IEnumerable<IPieceModel> Attackers();
        IEnumerable<IPieceModel> Defenders();
        IEnumerable<IPieceModel> Neareby(int distance);
    }
}
