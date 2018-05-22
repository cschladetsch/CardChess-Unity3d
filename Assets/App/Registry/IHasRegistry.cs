namespace App.Registry
{
    public interface IHasRegistry<IBase>
        where IBase : class, IHasDestroyHandler<IBase>, IKnown
    {
        IRegistry<IBase> Registry { get; set; }
    }
}
