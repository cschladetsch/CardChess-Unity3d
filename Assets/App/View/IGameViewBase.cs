using App.Model;
using Dekuple.View;

namespace App.View
{
    /// <inheritdoc />
    /// <summary>
    /// Totally unsure what this is used for
    /// </summary>
    public interface IGameViewBase
        : IViewBase
    {
        IPlayerView PlayerView { get; }
        IPlayerModel PlayerModel { get; }
        IArbiterView ArbiterView { get; set; }
        IBoardView BoardView { get; set; }
    }
}