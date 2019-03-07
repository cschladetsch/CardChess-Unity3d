using System;

namespace Dekuple.Agent
{
    using Common.Message;

    public class Turnaround
    {
        public IRequest Request;
        public Action<IResponse> Responder;

        public Turnaround(IRequest request, Action<IResponse> responder)
        {
            Request = request;
            Responder = responder;
        }
    }
}
