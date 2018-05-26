namespace App.Model
{
    using Common.Message;

    public interface IDecidingModelBase
    {
        Response NotImplemented(IRequest req, string text = "");
        Response Failed(IRequest req, string text = "", EError error = EError.Error);
    }
}
