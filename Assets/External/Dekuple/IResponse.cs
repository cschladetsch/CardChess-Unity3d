using System;

namespace Dekuple
{
    public interface IResponse
    {
        IRequest Request { get; set; }
        EResponse Type { get; }
        EError Error { get; }
        Guid RequestId { get; }
        string Text { get;}
        object PayloadObject { get; }
        bool Failed { get; }
        bool Success { get; }
    }

    public interface IResponse<TPayload>
        : IResponse
    {
        TPayload Payload { get; }
    }
}
