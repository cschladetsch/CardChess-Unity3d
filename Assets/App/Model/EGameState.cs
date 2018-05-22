namespace App.Model
{
    public enum EGameState
    {
        None,
        Shuffling,
        Dealing,
        Mulligan,

        PlaceKing,
        Ready,

        TurnStart,
        TurnPlay,
        Battle,
        TurnEnd,

        Completed,
    }
}
