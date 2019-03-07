using Dekuple.Model;
using UniRx;

namespace Dekuple.Common
{
    /// <summary>
    /// Indicates that an object is owned by a player.
    /// </summary>
    public interface IOwned
    {
        IReadOnlyReactiveProperty<IOwner> Owner { get; }
        IPlayerModel PlayerModel { get; }

        void SetOwner(IOwner owner);
    }
}
