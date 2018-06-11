using System;

namespace App.View
{
    using Agent;
    using Common.Message;

    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
