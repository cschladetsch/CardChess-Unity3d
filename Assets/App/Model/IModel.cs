namespace App.Model
{
    using Common;

    public delegate void ModelDestroyedHandler(object sender, IModel model, params object[] context);

    public interface IModel :
        IOwned,
        Flow.ILogger,
        IHasName,
        IHasId
    {
        event ModelDestroyedHandler OnDestroy;

        bool Destroyed { get; }
        Registry Registry { get; /* private */ set; }
        void Destroy();
    }
}
