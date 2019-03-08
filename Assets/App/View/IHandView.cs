using Dekuple.View;

namespace App.View
{
    using Agent;

    /// <summary>
    /// View of the hand of a player.
    /// </summary>
    public interface IHandView
        : IView<IHandAgent>
    {
    }
}
