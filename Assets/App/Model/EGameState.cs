namespace App.Model
{
    public enum EGameState
    {
        None,
        Start,
        Mulligan,

        PlaceKing,

        TurnStart,
        TurnPlay,
        Battle,
        TurnEnd,

        Completed,
    }
}
