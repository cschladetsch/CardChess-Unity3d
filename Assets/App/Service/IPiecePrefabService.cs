using Dekuple.View;
using UnityEngine;

namespace App.Service
{
    using Common;
    using Model;
    using View;

    public interface IPiecePrefabService
        : IViewBase
    {
        ICardView NewCard(IPlayerModel model, EPieceType type);
        IPieceView NewPiece(ICardModel model);

        Object GetCardPrefab(EPieceType type);
    }
}
