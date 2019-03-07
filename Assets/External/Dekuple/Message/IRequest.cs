namespace Dekuple.Common.Message
{
    using Model;

    /// <summary>
    /// An action proposed by a player.
    /// </summary>
    public interface IRequest
        : IHasId
    {
        IPlayerModel Player { get; }
        EActionType Action { get; }
    }
}
