using System;

namespace App.Common.Message
{
    public interface IResponse
    {
        IRequest Request { get; }
        EResponse Type { get; }
        EError Error { get; }
        Guid RequestId { get; }
        string Text { get;}
        object PayloadObject { get; }
        bool Failed { get; }
        bool Success { get; }
    }

    public interface IResponse<TPayload>
    {
        TPayload Payload { get; }
    }
}
