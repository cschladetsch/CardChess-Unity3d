namespace App.Model
{
    using Dekuple.Model;
    using Common;

    /// <inheritdoc cref="ICardCollection{TCard}" />
    /// <summary>
    /// A pre-made collection of cards used to make a deck.
    /// </summary>
    public interface ITemplateDeck
        : IModel
        , ICardCollection<ICardTemplate>
    {
    }
}
