using Flow;

namespace Dekuple.Common
{
    /// <summary>
    /// An actor that changes state at the start or end of a game.
    /// </summary>
    public interface IGameActor
    {
        void StartGame();
        void EndGame();
    }
}
