using System.Collections.Generic;
using App.Action;
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
        void Endame();

        Response Arbitrate(Action.IRequest request);

        #endregion
    }
}
