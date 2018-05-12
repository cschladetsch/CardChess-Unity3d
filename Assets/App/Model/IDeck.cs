namespace App.Model
{
    public interface ITemplateDeck : ICardCollection<ICardTemplate>, IHasId
    {
    }

    public interface IDeck : ICardCollection<ICardInstance>, ICreated
    {
        void NewGame();
        void Shuffle();
        ICardInstance Draw();
        void AddToBottom(ICardInstance card);
        void AddToRandom(ICardInstance card);
    }
}
