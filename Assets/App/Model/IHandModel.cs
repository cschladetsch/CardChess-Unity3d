namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// Cards that a player may choose to play during
    /// his turn.
    /// </summary>
    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
        , IGameActor
    {
        ICardModel this[int index] { get; set; }
    }
}
