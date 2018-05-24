
namespace App.Agent
{
	using Model;

    public interface ICardAgent :
        IAgent<ICardModel>,
        Common.ICard
    {
    }
}
