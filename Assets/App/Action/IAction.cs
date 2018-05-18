using App.Model;

namespace App.Action
{
    /// <summary>
    /// An action proposed by a player.
    /// </summary>
    public interface IAction
    {
        IPlayerModel Player { get; }
        EAction Type { get; }
    }
}
