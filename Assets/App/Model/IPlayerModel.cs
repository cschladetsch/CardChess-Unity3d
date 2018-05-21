using System;
using System.Collections.Generic;

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

        #region Gameplay Methods
        Response DrawHand();
        Response AcceptHand();
        Response PlayCard(ICardModel model, Coord coord);
        Response MovePiece(ICardModel model, Coord coord);
        Response Pass();
        #endregion // Gameplay Methods

        #endregion // Methods
    }
}
