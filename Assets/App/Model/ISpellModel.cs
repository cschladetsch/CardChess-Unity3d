using App.Action;
using Flow;

namespace App.Model.Card
{
    public interface ISpellModel
        : ICardModel
    {
        IGenerator Cast(Coord where);
        IGenerator Cast();
    }
}
