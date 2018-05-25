namespace App.Model
{
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
