namespace App.Common
{
    public interface IOwner : IHasId
    {
        EColor Color { get; }
        bool IsWhite { get; }
        bool IsBlack { get; }
    }
}
