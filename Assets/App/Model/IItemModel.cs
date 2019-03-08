namespace App.Model
{
    using Common.Message;

    /// <inheritdoc />
    /// <summary>
    /// An Item that may be placed on a piece on the board.
    /// </summary>
    public interface IItemModel
        : ICardModel
    {
        /// <summary>
        /// Try to equip this item onto given piece
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        Response TryEquip(IPieceModel piece);

        /// <summary>
        /// Try to Unequip item from given piece
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        Response TryUnequip(IPieceModel piece);
    }
}
