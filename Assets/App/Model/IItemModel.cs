namespace App.Model.Card
{
    public interface IItemModel
        : ICardModel
    {
        bool Equip(ICardModel target);
        bool UnEquip(ICardModel target);
    }
}
