namespace App.Model
{
    using UniRx;
    using Dekuple.Model;

    /// <inheritdoc />
    /// <summary>
    /// State for end-turn button for a Player.
    /// </summary>
    public interface IEndTurnButtonModel
        : IModel
    {
        IReadOnlyReactiveProperty<bool> Interactive { get; }
        IReadOnlyReactiveProperty<bool> PlayerHasOptions { get; }
    }
}
