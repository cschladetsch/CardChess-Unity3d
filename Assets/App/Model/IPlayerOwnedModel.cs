namespace App.Model
{
    using Common;

    public interface IPlayerOwnedModel
		: IModel
    {
        IPlayerModel Player { get; }
        EColor Color { get;}
    }
}
