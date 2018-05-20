namespace App.Model
{
    using Common;

    public delegate void ModelDestroyedHandler(object sender, IModel model, params object[] context);

    public interface IModel
        : IOwned
        , Flow.ILogger
        , IHasName
        , IHasId
    {
        event ModelDestroyedHandler OnDestroy;

        bool Destroyed { get; }
        // NOTE: we should not be able to change the registry of a model.
        // the setter is added to avoid having to use (slow) reflection to set it.
        // this may be changed later.
        IModelRegistry Registry { get; /* private */ set; }
        void Destroy();
    }
}
