using System;

namespace App.Common.Message
{
    public interface IResponse
    {
        EResponse Type { get; }
        EError Error { get; }
        Guid RequestId { get; }
        string Text { get;}
        object PayloadObject { get; }
    }

    public interface IResponse<TPayload>
    {
        TPayload Payload { get; }
    }
}
