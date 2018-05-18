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
    public interface IPlayerModel :
        IModel,
        IOwner
    {
        #region Properties
        int MaxMana { get; }
        int Mana { get; }
        int Health { get; }
        IHandModel HandModel { get; }
        IDeckModel DeckModel { get; }
        ICardModel King { get; }
        IEnumerable<ICardModel> CardsOnBoard { get; }
        IEnumerable<ICardModel> CardsInGraveyard { get; }
        #endregion

        #region Methods
        Response NewGame();
        Response ChangeMaxMana(int mana);
        Response ChangeMana(int mana);
        Response MockMakeHand();
        #endregion
    }
}
