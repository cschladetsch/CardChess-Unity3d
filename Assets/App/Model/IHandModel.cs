namespace App.Model
{
    using Common;

    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        Response NewGame();
    }
}
