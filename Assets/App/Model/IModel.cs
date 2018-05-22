namespace App.Model
{
    using Common;
    using Model;
    using Registry;

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
