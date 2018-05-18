using App.Model;

namespace App.Agent
{
    public interface ICardAgent :
        IAgent<Model.ICardModel>,
        Common.ICard
    {
    }
}
