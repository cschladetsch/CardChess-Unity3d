namespace App.Common
{
    /// <summary>
    /// Something that exists in the game. Can be a Model, an Agent, a View or
    /// anything else.
    ///
    /// IsValid must be true after Construct has been called. Else, the object
    /// will be deleted.
    ///
    /// IsReady must be true after Prepare has been called. Else, the object
    /// will be deleted.
    /// </summary>
    public interface IEntity
        : IHasId
        , IOwned
    {
        bool IsValid { get; }

        void Create();
    }
}
