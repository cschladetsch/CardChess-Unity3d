namespace App.Action
{
    /// <summary>
    /// The different actions a player can perform at certain times.
    /// </summary>
    public enum EActionType
    {
        RejectCards,
        AcceptCards,
        DrawCard,
        PlayCard,
        MovePiece,
        CastSpell,
        Battle,
        Pass,
        TurnEnd,
        Resign,
    }
}
