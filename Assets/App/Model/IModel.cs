namespace App.Model
{
    using Common;
    using Registry;

    /// <summary>
    /// Base for all persistent models.
    /// </summary>
    public interface IModel
        : Flow.ILogger
        , IEntity
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        bool Prepared { get; }

        /// <summary>
        /// Create other models required by this one.
        ///
        /// Should only be called once.
        /// </summary>
        void Prepare();
    }
}
