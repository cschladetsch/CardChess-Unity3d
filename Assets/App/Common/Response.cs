namespace App.Common
{
    public enum EResponse
    {
        Ok,
        Fail,
        TimedOut,
    }

    public enum EError
    {
        None,
        InvalidArgs,
		InvalidSource,
		InvalidTarget,
        OutOfSequence,
        NoChange
    }

    public interface IResponse
    {
        EResponse Type { get; }
        EError Error { get; }
        string Text { get;}
    }

    public interface IResponse<TPayload>
    {
        TPayload Payload { get; }
    }

    public class Response
        : IResponse
    {
        public EResponse Type { get; }
        public EError Error { get; }
        public string Text { get; }

		public static Response Ok = new Response(EResponse.Ok);
		public static Response Fail = new Response(EResponse.Fail);

        public Response(EResponse response = EResponse.Ok, EError err = EError.None)
        {
            Type = response;
            Error = EError.None;
        }
    }

    public class Response<TPayload>
        : Response
        , IResponse<TPayload>
    {
        public TPayload Payload { get; }

        public Response(TPayload load, EResponse response = EResponse.Ok, EError err = EError.None)
            : base(response, err)
        {
            Payload = load;
        }
    }
}
