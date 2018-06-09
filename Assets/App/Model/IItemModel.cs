namespace App.Model
{
    using Common.Message;

    /// <summary>
    /// An Item that may be placed on a piece on the board.
    /// </summary>
    public interface IItemModel
        : ICardModel
    {
        Response TryEquip(IPieceModel piece);
        Response TryUnequip(IPieceModel piece);
    }
}
