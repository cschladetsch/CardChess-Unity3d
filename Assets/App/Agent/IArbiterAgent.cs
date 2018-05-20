using App.Common;
using App.Model;

namespace App.Agent
{
    public interface IArbiterAgent :
        IAgent<Model.IArbiterModel>
    {
        ICardAgent NewCardAgent(ICardModel cardModel, IOwner owner);
    }
}
