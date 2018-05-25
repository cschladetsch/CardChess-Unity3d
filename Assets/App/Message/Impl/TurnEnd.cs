using UnityEngine.Assertions;

namespace App.Action
{
    using Model;
    using Common;

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
