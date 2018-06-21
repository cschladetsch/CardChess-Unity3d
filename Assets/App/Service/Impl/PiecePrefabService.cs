using System.Linq;
using UnityEngine;

namespace App.Service.Impl
{
    using Common;
    using Model;
    using View;
    using View.Impl1;

    public class PiecePrefabService
        : ViewBase
        , IPiecePrefabService
    {
        public CardView[] Cards;
        public PieceView[] Pieces;

        public ICardView NewCard(IPlayerModel model, EPieceType type)
        {
            throw new System.NotImplementedException();
        }

        public IPieceView NewPiece(ICardModel model)
        {
            throw new System.NotImplementedException();
        }

        public Object GetCardPrefab(EPieceType type)
        {
            return Cards.FirstOrDefault(p => p.PieceType == type);
        }

    }
}
