using System;

namespace Dekuple.View
{
    using Agent;
    using Common.Message;

    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        void SetAgent(IPlayerView view, IPlayerAgent agent);
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
