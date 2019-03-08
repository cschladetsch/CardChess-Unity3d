using Dekuple.Model;
using UniRx;

namespace App.Model
{
    public interface IEndTurnButtonModel
        : IModel
    {
        IReadOnlyReactiveProperty<bool> Interactive { get; }
        IReadOnlyReactiveProperty<bool> PlayerHasOptions { get; }
    }
}
