using UniRx;

namespace App.Model
{
    /// <inheritdoc />
    /// <summary>
    /// A card model that is able to be ridden by another piece.
    /// </summary>
    public interface ICardModelMountable
        : ICardModel
    {
        ReadOnlyReactiveProperty<ICardModel> Rider { get; }

        bool CanMount(ICardModel other);
        bool SetMounted(ICardModel rider);
        bool Dismount();
    }
}
