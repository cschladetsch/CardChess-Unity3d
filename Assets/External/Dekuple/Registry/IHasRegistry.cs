namespace Dekuple.Registry
{
    public interface IHasRegistry<IBase>
        where IBase
            : class
            , IHasId
            , IHasDestroyHandler<IBase>
    {
        IRegistry<IBase> Registry { get; set; }
    }
}
