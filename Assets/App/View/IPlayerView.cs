using System;

namespace App.View
{
    using Agent;
    using Common.Message;

    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        void NewRequest(IRequest request, Action<IResponse> response);
    }
}
