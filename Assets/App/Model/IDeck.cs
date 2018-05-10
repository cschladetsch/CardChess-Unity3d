namespace App.Model
{
    public interface IDeck : ICardCollection
    {
        ICardInstance Draw();
        void Shuffle();
    }
}