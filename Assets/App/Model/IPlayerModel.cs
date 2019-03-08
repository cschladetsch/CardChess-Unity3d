using System;
using App.Common;
using App.Common.Message;
using Dekuple;
using Dekuple.Model;
using UniRx;

namespace App.Model
{
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

        IRequest NextAction();
        Response CardDrawn(ICardModel card);
        void CardExhaustion();
        void StartTurn();
        void EndTurn();
        void Result(IRequest req, IResponse response);
        Response ChangeMana(int manaChange);
        ICardModel RandomCard();
    }
}
