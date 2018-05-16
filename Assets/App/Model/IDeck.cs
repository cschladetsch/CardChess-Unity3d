using System;

namespace App.Model
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// A deck in play in a game
    /// </summary>
    public interface IDeck :
        ICardCollection<ICardInstance>,
        ICreateWith<ITemplateDeck>,
        IOwned
    {
        void NewGame();
        void Shuffle();
        ICardInstance Draw();
        void AddToBottom(ICardInstance card);
        void AddToRandom(ICardInstance card);
    }
}
