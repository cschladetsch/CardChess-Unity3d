using App.Common;
using App.Model;

namespace App.Action
{
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
