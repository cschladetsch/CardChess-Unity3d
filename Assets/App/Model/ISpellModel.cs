namespace App.Model.Card
{
    using Common;

    public interface ISpellModel
        : ICardModel
    {
        Response Cast(Coord where);
        Response Cast();
    }
}
