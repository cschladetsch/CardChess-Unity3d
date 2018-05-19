using Flow;

namespace App.Model.Card
{
    using Common;

    public interface ISpellModel
        : ICardModel
    {
        IGenerator Cast(Coord where);
        IGenerator Cast();
    }
}
