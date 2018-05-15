namespace App
{
    /// <summary>
    /// Global game parameters.
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Number of cards to start with (excluding King)
        /// </summary>
        public static int StartHandCardCount = 6;

        public static int MaxHandCount = 10;

        public static int MaxCardsInDeck = 60;

        public static int MinCardsInDeck = 30;

        /// <summary>
        /// The maximum mana a player can have
        /// </summary>
        public static int MaxManaCap = 14;

        /// <summary>
        /// How long in seconds a player has to place King on board
        /// </summary>
        public static float PlaceKingTimer = 10;

        /// <summary>
        /// How long in seconds a player has to form final hand from initial cards
        /// </summary>
        public static float MulliganTimer = 20;

        /// <summary>
        /// How long a player has to complete his turn. Note that there can be
        /// many actions in a turn, and the ordering is important.
        /// </summary>
        public static float GameTurnTimer = 45;
    }
}
