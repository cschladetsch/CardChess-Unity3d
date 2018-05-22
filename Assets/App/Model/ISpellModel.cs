namespace App.Model.Card
{
    using Common;

    public interface ISpellModel
        : ICardModel
    {
        EResponse Cast(Coord where);
        EResponse Cast();
    }
}
