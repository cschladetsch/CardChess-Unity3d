namespace Dekuple
{
    using Model;

    /// <summary>
    /// An action proposed by a player.
    /// </summary>
    public interface IRequest
        : IHasId
    {
        IOwner Owner { get; }
        //EActionType Action { get; }
    }
}
