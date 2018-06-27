using App.Model;

namespace App.Agent
{
    /// <summary>
    /// Agent for EndTurn button.
    ///
    /// This is currently pass-through. Later will be used for animation state and remoting.
    /// </summary>
    public interface IEndTurnButtonAgent
        : IAgent<IEndTurnButtonModel>
    {
    }
}
