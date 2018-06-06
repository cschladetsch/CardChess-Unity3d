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
        /// Create other models required by this one.
        /// </summary>
        void Prepare();
    }
}
