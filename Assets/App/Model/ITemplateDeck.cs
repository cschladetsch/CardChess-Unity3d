namespace App.Model
{
    using Common;

    /// <summary>
    /// A pre-made deckModel
    /// </summary>
    public interface ITemplateDeck :
        IModel,
        ICardCollection<ICardTemplate>
    {
    }
}
