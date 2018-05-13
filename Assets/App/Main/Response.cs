namespace App
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
    }

    public class Response
    {
        public EResponse Type;
        public EError Error;
        public string Text { get; }
    }
}
