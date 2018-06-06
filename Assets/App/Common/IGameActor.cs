using Flow;

namespace App.Common
{
    /// <summary>
    /// An actor that changes state at the start or end of a game.
    /// </summary>
    public interface IGameActor
    {
        void NewGame();
        void EndGame();
    }
}
