using Dekuple;

namespace App.Common.Message
{
    using Model;

    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's Hand onto the Board
    /// </summary>
    public class TurnEnd
        : RequestBase
    {
        public TurnEnd(IPlayerModel player)
            : base(player, EActionType.TurnEnd)
        {
            Assert.IsNotNull(player);
        }

        public override string ToString()
        {
            return $"{Player} ends turn";
        }
    }
}
