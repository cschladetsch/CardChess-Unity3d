namespace App.Model
{
    using Common;

    public interface IModel :
        IOwned,
        Flow.ILogger,
        IHasName,
        IHasId
    {
    }
}
