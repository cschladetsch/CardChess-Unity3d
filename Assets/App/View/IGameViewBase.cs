namespace App.View
{
    using Dekuple.View;
    using Model;

    /// <inheritdoc />
    /// <summary>
    /// Used for things that generally need to know about the game state.
    ///
    /// TODO: Remove this. It's unnecessary.
    /// </summary>
    public interface IGameViewBase
        : IViewBase
    {
        IPlayerView PlayerView { get; set; }
        IPlayerModel PlayerModel { get; }
        IArbiterView ArbiterView { get; set; }
        IBoardView BoardView { get; set; }
    }
}