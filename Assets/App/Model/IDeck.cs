using System;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A deck in play in a game
    /// </summary>
    public interface IDeck :
        ICardCollection<ICard>,
        ICreateWith<ITemplateDeck>,
        IModel
    {
        void NewGame();
        void Shuffle();
        ICard Draw();
        bool AddToBottom(ICard card);

        /// <summary>
        /// Adds a number of cards to random locations in the deck.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>the number of cards added</returns>
        int ShuffleIn(params ICard[] card);
    }
}
