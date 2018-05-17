namespace App.Model
{
    using Common;

    /// <summary>
    /// A pre-made deck
    /// </summary>
    public interface ITemplateDeck :
        IModel,
        ICardCollection<ICardTemplate>
    {
    }
}
