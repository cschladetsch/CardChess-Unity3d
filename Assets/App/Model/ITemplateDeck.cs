using Dekuple.Model;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A pre-made collection of cards used to make a deck.
    /// </summary>
    public interface ITemplateDeck
        : IModel
        , ICardCollection<ICardTemplate>
    {
    }
}
