namespace App.Common
{
    public interface IOwned
    {
        IOwner Owner { get; }

        bool SameOwner(IOwner other);
    }
}
