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
        /// <summary>
        /// If true, this model has already been prepared.
        /// </summary>
        bool Prepared { get; }

        /// <summary>
        /// Create all other models required by this one.
        ///
        /// Should only be called once.
        /// </summary>
        void Prepare();
    }
}
