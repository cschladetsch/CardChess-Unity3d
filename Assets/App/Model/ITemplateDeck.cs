namespace App.Model
{
    /// <inheritdoc />
    /// <summary>
    /// A pre-made deck
    /// </summary>
    public interface ITemplateDeck :
        ICardCollection<ICardTemplate>,
        IHasId
    {
    }
}
