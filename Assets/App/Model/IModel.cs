namespace App.Model
{
    using Common;
    using Dekuple.Registry;
    using Dekuple.Common;

    /// <summary>
    /// Base for all persistent models.
    /// </summary>
    public interface IModel
        : Dekuple.Model.IModel
        , IHasDestroyHandler<IModel>
        , IHasRegistry<IModel>
    {
        ///// <summary>
        ///// If true, this model has already been prepared.
        ///// </summary>
        //bool Prepared { get; }

        ///// <summary>
        ///// Create all other models required by this one.
        /////
        ///// Should only be called once.
        ///// </summary>
        //void PrepareModels();
    }
}
