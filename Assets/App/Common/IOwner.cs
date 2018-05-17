namespace App.Common
{
    public interface IOwner : IHasId
    {
        EColor Color { get; }
    }
}
