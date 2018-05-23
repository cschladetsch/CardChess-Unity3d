using System.Collections.Generic;
using App.Common;
using JetBrains.Annotations;

namespace App.Model
{
    public interface IArbiterModel
        : IModel
    {
        #region Properties

        IBoardModel Board { get; }
        IPlayerModel WhitePlayer { get; }
        IPlayerModel BlackPlayer { get; }
        IPlayerModel CurrentPlayer { get; }
        IPlayerModel OtherPlayer { get; }
        EGameState GameState { get; }

        #endregion

        #region Methods

        void NewGame(IPlayerModel white, IPlayerModel black);
        void PlayerMulligan(IPlayerModel player, IEnumerable<ICardModel> cards);
        void PlayerAcceptCards(IPlayerModel player);
        void Endame();

        Response RequestPlayCard(IPlayerModel player, ICardModel card);
        Response RequestPlayCard(IPlayerModel player, ICardModel card, Coord coord);
        Response RequestMovePiece(IPlayerModel player, IPieceModel piece, Coord coord);

        #endregion
    }
}
