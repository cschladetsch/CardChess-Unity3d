namespace App.Model
{
    /// <summary>
    /// Different states that the Arbiter can be in.
    /// </summary>
    public enum EGameState
    {
        None,
        Start,
        Mulligan,
        PlaceKing,
        PlayTurn,
        Battle,
        TurnEnd,
        Completed,
    }
}
