namespace App.Common
{
    public interface IMountable : ICard
    {
        ICard Rider { get; }
        bool Mounted { get;}

        bool CanMount(ICard other);
    }
}
