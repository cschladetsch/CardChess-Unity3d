// define this for some diagnostics
#define LOG_TRACE_VERBOSE

// ReSharper disable InconsistentNaming

namespace App
{
    /// <summary>
    /// Global game parameters.
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// DEfault logging values.
        /// </summary>
#if LOG_TRACE_VERBOSE
        //public static bool DefaultShowTraceStack = true;
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 100;
#else
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 4;
#endif

        /// <summary>
        /// Number of cards to start with (excluding King)
        /// </summary>
        public static int StartHandCardCount = 6;
        public static int MaxCardsInHand = 10;
        public static int MaxCardsInDeck = 60;
        public static int MinCardsInDeck = 30;

        /// <summary>
        /// The maximum mana a playerAgent can have
        /// </summary>
        public static int MaxManaCap = 14;

        /// <summary>
        /// How long in seconds a playerAgent has to place King on boardAgent
        /// </summary>
        public static float PlaceKingTimer = 10;

        /// <summary>
        /// How long in seconds a playerAgent has to form final hand from initial cards
        /// </summary>
        public static float MulliganTimer = 20;

        /// <summary>
        /// How long a playerAgent has to complete his turn. Note that there can be
        /// many actions in a turn, and the ordering is important.
        /// </summary>
        public static float GameTurnTimer = 45;

        /// <summary>
        /// Minimum distance to enemy king when playing a new card to the boardAgent
        /// </summary>
        public static int EnemyKingClosestPlacement = 4;
    }
}
