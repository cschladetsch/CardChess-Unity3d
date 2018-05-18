namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// A valid description of a movement of a piece on the BoardAgent.
    /// </summary>
    public class MovePiece : ActionBase
    {
        public Agent.ICardAgent Instance;
        public Coord Target;
    }
}
