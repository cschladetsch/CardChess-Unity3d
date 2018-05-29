using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;

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
        IReactiveProperty<Coord> Coord { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReactiveProperty<bool> Dead { get; }

        Response Attack(IPieceModel piece);
        Response TakeDamage(IPieceModel piece);
    }
}
