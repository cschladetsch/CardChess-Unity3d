using App.Agent;
using UnityEditor;

namespace App.View.Impl1
{
    public class ArbiterView
        : ViewBase<IArbiterAgent>
    {
        public BoardView Board;
        public HandView WhiteHand;
        public HandView BlackHand;
        public DeckView WhiteDeck;
        public DeckView BlackDeck;
        public TMPro.TextMeshPro CurrentPlayerText;
    }
}
