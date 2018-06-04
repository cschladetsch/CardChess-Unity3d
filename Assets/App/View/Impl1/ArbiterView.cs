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
        public GameRoot Root;

        protected override void Begin()
        {
            base.Begin();

            WhiteHand.Construct(Root.WhiteAgent.Hand);
            BlackHand.Construct(Root.BlackAgent.Hand);
            WhiteDeck.Construct(Root.WhiteAgent.Deck);
            BlackDeck.Construct(Root.BlackAgent.Deck);
        }
    }
}
