namespace App.Model
{
    using Dekuple.Model;
    using Common;

    /// <inheritdoc cref="ICardCollection{TCard}" />
    /// <summary>
    /// Cards that a player may choose to play during his turn.
    /// </summary>
    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
        , IGameActor
    {
        ICardModel this[int index] { get; set; }
    }
}
