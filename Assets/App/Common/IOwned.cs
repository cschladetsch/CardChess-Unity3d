using UniRx;

namespace App.Common
{
    public interface IOwned
    {
        IReadOnlyReactiveProperty<IOwner> Owner { get; }
    }
}
