namespace App.Model.Card
{
    using Common;
    using Common.Message;

    public interface ISpellModel
        : ICardModel
    {
        Response Cast(Coord where);
        Response Cast();
    }
}
