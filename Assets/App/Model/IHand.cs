namespace App.Model
{
    using Common;

    public interface IHand : ICardCollection<ICardInstance>
    {
        void NewGame();
    }
}
