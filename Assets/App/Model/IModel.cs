using UniRx;

namespace App.Model
{
    using Common;
    using Registry;

    /// <summary>
    /// Base for all persistent models.
    /// </summary>
    public interface IModel
        : Flow.ILogger
        , IKnown
        , IOwned
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        IReadOnlyReactiveProperty<bool> Destroyed { get; }
        void Destroy();
    }
}
