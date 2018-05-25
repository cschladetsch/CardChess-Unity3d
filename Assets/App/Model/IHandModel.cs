namespace App.Model
{
    using Common;

    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        ICardModel this[int index] { get; set; }
        Response NewGame();
    }
}
