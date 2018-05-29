using UniRx;

namespace App.Model
{
    public interface ICardModelMountable
        : ICardModel
    {
        ReadOnlyReactiveProperty<ICardModel> Rider { get; }
        bool CanMount(ICardModel other);
        bool SetMounted(ICardModel rider);
    }
}
