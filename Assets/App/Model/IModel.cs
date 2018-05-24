namespace App.Model
{
    using Common;
    using Registry;

    public interface IModel
        : Flow.ILogger
        , IKnown
        , IOwned
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        bool Destroyed { get; }
        void Destroy();
    }
}
