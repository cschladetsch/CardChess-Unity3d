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
        OutOfSequence,
        NoChange
    }

    public class Response
    {
        public EResponse Type;
        public EError Error;
        public string Text;

        public static Response Ok = new Response();

        public Response(EResponse response = EResponse.Ok, EError err = EError.None)
        {
            Type = response;
            Error = EError.None;
        }
    }
}
