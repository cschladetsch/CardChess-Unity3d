namespace App.Model
{
    using Common;

    public interface IPlayerOwnedModel
		: IModel, IOwned
    {
        IPlayerModel Player { get; }
        EColor Color { get;}
        bool IsWhite { get; }
        bool IsBlack { get; }
    }
}
