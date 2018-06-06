namespace App.Model.Card
{
    using Common;
    using Common.Message;

    /// <summary>
    /// Model for a spell that can be cast during a plauyer's turn.
    /// </summary>
    public interface ISpellModel
        : ICardModel
    {
        /// <summary>
        /// Cast at a particular coordinate. This may or may not have a piece on it.
        /// </summary>
        /// <param name="where"></param>
        /// <returns>True if the cast succeeded</returns>
        Response Cast(Coord where);

        /// <summary>
        /// Cast a spell that doesn't require a specific target.
        /// </summary>
        /// <returns>True if the cast succeeded</returns>
        Response Cast();
    }
}
