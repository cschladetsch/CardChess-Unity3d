namespace App.Model
{
    using Common;

    public interface IHand :
        IModel,
        ICardCollection<ICard>
    {
        void NewGame();
    }
}
