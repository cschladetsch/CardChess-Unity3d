using System;

namespace Dekuple
{
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
