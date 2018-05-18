namespace App.Model
{
    public interface ICardModelMountable
        : ICardModel
    {
        ICardModel Rider { get; }
        bool CanMount(ICardModel other);
        bool SetMounted(ICardModel rider);
    }
}
