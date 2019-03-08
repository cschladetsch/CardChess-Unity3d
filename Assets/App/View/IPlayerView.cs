using System;
using Dekuple;
using Dekuple.View;

namespace App.View
{
    using Agent;

    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        void SetAgent(IPlayerView view, IPlayerAgent agent);
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
