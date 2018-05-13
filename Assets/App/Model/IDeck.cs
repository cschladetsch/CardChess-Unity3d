using System;

namespace App.Model
{
    /// <summary>
    /// A pre-made deck
    /// </summary>
    public interface ITemplateDeck :
        ICardCollection<ICardTemplate>,
        IHasId
    {
    }

    /// <summary>
    /// A deck in play in a game
    /// </summary>
    public interface IDeck :
        ICardCollection<ICardInstance>,
        ICreated<ITemplateDeck>,
        IOwned
    {
        void NewGame();
        void Shuffle();
        ICardInstance Draw();
        void AddToBottom(ICardInstance card);
        void AddToRandom(ICardInstance card);
    }
}
