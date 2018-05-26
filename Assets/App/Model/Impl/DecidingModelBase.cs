using System.IO.Ports;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// Base for classes that have methods that produce methods returning Response
    /// </summary>
    public class DecidingModelBase
        : ModelBase
        , IDecidingModelBase
    {
        protected DecidingModelBase()
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

            Error(text);
            req.Player.RequestFailed(req);
            return new Response(EResponse.Fail, error, text);
        }
    }
}
