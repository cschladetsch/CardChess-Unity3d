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
        public static int StartHandCardCount = 7;

        /// <summary>
        /// The maximum mana a player can have
        /// </summary>
        public static int MaxManaCap = 12;

        /// <summary>
        /// How long a player has to place King on board
        /// </summary>
        public static float PlaceKingTimer = 10;

        /// <summary>
        /// How long a player has to form final deck from initial cards
        /// </summary>
        public static float MulliganTimer = 20;

        /// <summary>
        /// How long a player has to complete his turn
        /// </summary>
        public static float GameTurnTimer = 45;
    }
}
