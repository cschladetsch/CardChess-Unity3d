using Dekuple;

namespace App.Model
{
    using Common.Message;

    /// <inheritdoc cref="CardModel" />
    /// <summary>
    /// An Item that can be placed on a Piece on the Board.
    /// </summary>
    public class ItemModel
        : CardModel
        , IItemModel
    {
        public ItemModel(IOwner owner, ICardTemplate template)
            : base(owner, template)
        {
        }

        public Response TryEquip(IPieceModel piece)
        {
            throw new System.NotImplementedException();
        }

        public Response TryUnequip(IPieceModel piece)
        {
            throw new System.NotImplementedException();
        }
    }
}
