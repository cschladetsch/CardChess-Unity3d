namespace App.Model
{
    using Common;

    /// <summary>
    /// Cards that a player may choose to play during his turn.
    /// </summary>
    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        ICardModel this[int index] { get; set; }
    }
}
