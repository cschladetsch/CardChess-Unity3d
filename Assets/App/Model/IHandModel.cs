namespace App.Model
{
    using Common;
    using Common.Message;

    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        ICardModel this[int index] { get; set; }
        Response NewGame();
    }
}
