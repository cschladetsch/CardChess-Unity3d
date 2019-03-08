using System;
using App.Common;
using App.Common.Message;
using Dekuple;
using Dekuple.Model;
using UniRx;

namespace App.Model
{
    /// <inheritdoc cref="IModel" />
    /// <summary>
    /// A Player in the game.
    /// Hopefully, these could be bots, or remote players as well
    /// as simple hotseat players at the same local device.
    /// </summary>
    public interface IPlayerModel
        : IModel
        , IOwner
        , IGameActor
    {
        event Action<IPieceModel, IItemModel> OnEquipped;
        event Action<IPieceModel, IItemModel> OnUnequipped;
        event Action<ISpellModel, IModel> OnCastSpell;

        IBoardModel Board { get; }
        IArbiterModel Arbiter { get; }
        IDeckModel Deck { get; }
        IHandModel Hand { get; }
        IEndTurnButtonModel EndTurnButton { get; }
        IReactiveProperty<int> MaxMana { get; }
        IReactiveProperty<int> Mana { get; }
        IReactiveProperty<int> Health { get; }
        EColor Color { get; }

        /// <summary>
        /// Get the next action from this player
        /// </summary>
        /// <returns></returns>
        IGameRequest NextAction();

        /// <summary>
        /// Response to drawing a given card
        /// </summary>
        /// <param name="card">the card drawn</param>
        /// <returns>the response</returns>
        Response CardDrawn(ICardModel card);

        /// <summary>
        /// Invoked when the player tries to draw with an empty hand
        /// </summary>
        void CardExhaustion();

        void StartTurn();

        void EndTurn();

        void Result(IRequest req, IResponse response);

        /// <summary>
        /// What happens when mana changes for this player
        /// </summary>
        /// <param name="manaChange">How much the mana changes</param>
        /// <returns>A response to the change</returns>
        Response ChangeMana(int manaChange);

        /// <summary>
        /// Get a random card from the player
        /// </summary>
        /// <returns>A random card from this player's hand</returns>
        ICardModel RandomCard();
    }
}
