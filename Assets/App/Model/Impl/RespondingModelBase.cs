using System.IO.Ports;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// Base for classes that have methods that produce methods returning Response
    /// </summary>
    public class RespondingModelBase
        : ModelBase
        , IDecidingModelBase
    {
        protected RespondingModelBase(IOwner owner)
            : base(owner)
        {
            LogSubject = this;
        }

        public Response NotImplemented(IRequest req, string text = "")
        {
            return Failed(req, $"Not Implemented {text}", EError.NotImplemented);
        }

        public Response Failed(IRequest req, string text = "", EError error = EError.Error)
        {
            Assert.IsNotNull(req);
            Assert.IsNotNull(req.Player);

            var resp = new Response(EResponse.Fail, error, text);
            Verbose(10, "Request {req} failed: {resp}");
            return resp;
        }
    }
}
