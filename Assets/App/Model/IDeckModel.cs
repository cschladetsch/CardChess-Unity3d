using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A deckModel in play in a game
    /// </summary>
    public interface IDeckModel
        : ICardCollection<ICardModel>
        , IConstructWith<ITemplateDeck>
        , IModel
    {
        void NewGame();
        void Shuffle();

        /// <summary>
        /// Draw from the top of the deck
        /// </summary>
        /// <returns></returns>
        ICardModel Draw();
        IEnumerable<ICardModel> Draw(int count);

        bool AddToBottom(ICardModel cardModel);

        /// <summary>
        /// Adds a number of cards to random locations in the deckModel.
        /// </summary>
        /// <param name="models"></param>
        /// <returns>the number of cards added, as there is a maximum size to a deck</returns>
        int ShuffleIn(params ICardModel[] models);
    }
}
