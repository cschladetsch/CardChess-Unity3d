using App.Model;

namespace App.Agent
{
    public interface ICard :
        IAgent<Model.ICard>,
        Common.ICard
    {
    }
}
