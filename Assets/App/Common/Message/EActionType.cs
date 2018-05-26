namespace App.Common.Message
{
    /// <summary>
    /// The different actions a player can perform at certain times.
    /// </summary>
    public enum EActionType
    {
        RejectCards,
        AcceptCards,
        DrawCard,
        PlacePiece,
        MovePiece,
        GiveItem,
        CastSpell,
        Mount,
        Battle,
        Pass,
        TurnEnd,
        Resign,
    }
}
