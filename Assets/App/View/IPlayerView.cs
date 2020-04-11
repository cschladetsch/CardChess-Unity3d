using System;

using Dekuple;
using Dekuple.Agent;
using Dekuple.View;

namespace App.View
{
    using Agent;

    /// <summary>
    /// View of a player in the scene
    /// </summary>
    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
