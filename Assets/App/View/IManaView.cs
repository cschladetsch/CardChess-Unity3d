
using Dekuple.View;

namespace App.View
{
    using Agent;

    /// <summary>
    /// View of the mana available to a player
    /// </summary>
    public interface IManaView
        : IView<IPlayerAgent>
    {
    }
}
