namespace App.Model
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// A collection of card templates. not used directly in a game.
    /// </summary>
    public class ModelDeckCollection : CardCollection<ICardTemplate>
    {
        public override int MaxCards => Parameters.MinCardsInDeck;
    }
}
