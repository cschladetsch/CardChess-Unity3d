namespace App.Model
{
    /// <inheritdoc />
    ///  <summary>
    ///  Model of an effect a player can cast.
    ///  Effects can be on areas of the board, or on pieces on the board.
    ///  </summary>
    public interface IEffectModel
        : ICardModel
    {
        /// <summary>
        /// The radius of the effect, in orthogonal coordinates.
        /// </summary>
        int Radius { get; }

        /// <summary>
        /// How long the effect stays.
        /// </summary>
        int NumTurns { get; }
    }
}
