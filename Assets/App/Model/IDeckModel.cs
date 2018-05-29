using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// The pool of cards that the player can draw from
    /// during a game.
    /// </summary>
    public interface IDeckModel
        : ICardCollection<ICardModel>
        , IConstructWith<ITemplateDeck, IOwner>
        , IModel
    {
        void NewGame();
        void Shuffle();
        ICardModel Draw();
        IEnumerable<ICardModel> Draw(int count);
        bool AddToBottom(ICardModel cardModel);
        int ShuffleIn(params ICardModel[] models);
    }
}
