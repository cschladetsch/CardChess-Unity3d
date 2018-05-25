using System;
using System.Collections.Generic;
using App.Action;

namespace App.Model
{
    using Common;

    /// <summary>
    /// PlayerAgent in the game.
    /// Hopefully, these could be bots, or remote players as well
    /// as simple hotseat players at the same local device.
    /// </summary>
    public interface IPlayerModel
        : IModel
        , IOwner
    {
        #region Properties
        IBoardModel Board { get; }
        IArbiterModel Arbiter { get; }

        IDeckModel Deck { get; }
        IHandModel Hand { get; }
        ICardModel King { get; }

        IEnumerable<ICardModel> CardsOnBoard { get; }
        int MaxMana { get; }
        int Mana { get; }
        int Health { get; }

        #endregion // Properties

        #region Methods
        Response NewGame();
        Response ChangeMaxMana(int mana);
        Response ChangeMana(int mana);

        Response DrawHand();
        void CardExhaustion();
        IRequest NextAction();

        Response CardDrawn(ICardModel card);

        /// <summary>
        /// Sent by Arbiter when an action failed.
        /// </summary>
        /// <param name="req"></param>
        void RequestFailed(IRequest req);

        /// <summary>
        /// Sent by Arbiter when action succeeded.
        /// </summary>
        /// <param name="req"></param>
        void RequestSuccess(IRequest req);

        //Response<IPieceModel> PlayCard(ICardModel card, Coord coord);
        //Response MoveCard(ICardModel card, Coord coord);

        #endregion // Methods

    }
}
