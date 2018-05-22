namespace App.Model
{
    using Common;

    public delegate void DestroyedHandler<T>(T model);

    public interface IHasDestroyHandler<T>
    {
        event DestroyedHandler<T> OnDestroy;
    }

    public interface IModel
        : Flow.ILogger
        , IKnown
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        bool Destroyed { get; }
        void Destroy();
    }
}
