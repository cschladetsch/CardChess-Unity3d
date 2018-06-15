namespace App.Model
{
    using Common.Message;

    /// <summary>
    /// Common for models that have to make decisions to return to the Arbiter
    /// </summary>
    public interface IDecidingModelBase
    {
        Response NotImplemented(IRequest req, string text = "");
        Response Failed(IRequest req, string text = "", EError error = EError.Error, EResponse r = EResponse.Fail);
        Response Succeed(IRequest req, string text = "", EError error = EError.None, EResponse r = EResponse.Ok);
    }
}
