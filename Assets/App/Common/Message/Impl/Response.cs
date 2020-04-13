using System;
using Dekuple;

namespace App.Common.Message
{
    public class Response
        : IResponse
    {
        public IRequest Request { get; set; }
        public EResponse Type { get; }
        public EError Error { get; }
        // public Guid RequestId { get; }
        public string Text { get; }
        public object PayloadObject { get; protected set; }
        public bool Failed => !Success;
        public bool Success => Type == EResponse.Ok && (Error == EError.None || Error == EError.NoChange);

        public static Response NotImplemented = new Response(EResponse.NotImplemented);
        public static readonly Response Ok = new Response();
        public static readonly Response Fail = new Response(EResponse.Fail);

        public Response(EResponse response = EResponse.Ok, EError err = EError.None, string text = "")
        {
            Type = response;
            Error = err;
            Text = text;
        }

        public Response(IRequest request, EResponse response = EResponse.Ok, EError err = EError.None,
            string text = "")
            : this(response, err, text)
        {
            Request = request;
        }

        //public override bool Equals(object obj)
        //{
        //    var r = obj as IResponse;
        //    if (r == null)
        //        return false;
        //    var eq = Request == r.Request && Type == r.Type && Error == r.Error && Text == r.Text;
        //    if (!eq)
        //        return false;
        //    if (PayloadObject != null)
        //        return PayloadObject.Equals(r.PayloadObject);
        //    return r.PayloadObject == null;
        //}

        public override string ToString()
        {
            // if (Error == EError.None)
            //     return $"{Request}->{Success} {Type}:{Text}; {PayloadObject}";
            // return $"{Request}->{Success} {Type}:{Error} {Text}; {PayloadObject}";
            if (Error == EError.None)
                return $"{Request} {Type}:{Text}";
            return $"{Request} {Type}:{Text}:{Error}";
        }
    }

    public class Response<TPayload>
        : Response
        , IResponse<TPayload>
    {
        public TPayload Payload { get; }

        public Response(TPayload load, EResponse response = EResponse.Ok, EError err = EError.None, string text = "")
            : base(response, err, text)
        {
            PayloadObject = Payload = load;
        }

        public static Response<TPayload> FailWith(string text)
        {
            return new Response<TPayload>(default(TPayload), EResponse.Fail, EError.Error, text);
        }
    }
}
