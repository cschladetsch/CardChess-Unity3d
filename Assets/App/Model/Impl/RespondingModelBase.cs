using Dekuple;
using Dekuple.Model;

namespace App.Model
{
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
            return Failed(req, $"{text}", EError.NotImplemented);
        }

        public Response Succeed(IRequest req, string text = "", EError error = EError.None, EResponse r = EResponse.Ok)
        {
            Assert.IsNotNull(req);
            Assert.IsNotNull(req.Owner as IPlayerModel);

            return new Response(req, r, error, text);
        }

        public Response Failed(IRequest req, string text = "", EError error = EError.Error, EResponse r = EResponse.Fail)
        {
            Assert.IsNotNull(req);
            Assert.IsNotNull(req.Owner as IPlayerModel);

            var resp = new Response(req, r, error, text);
            Warn($"{resp}");
            return resp;
        }
    }
}
