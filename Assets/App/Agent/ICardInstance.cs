using App.Model;

namespace App.Agent
{
    public interface ICardInstance :
        IAgent<Model.ICardInstance>,
        Common.ICard,
        Common.IOwned,
        Common.IHasName
    {
    }
}
