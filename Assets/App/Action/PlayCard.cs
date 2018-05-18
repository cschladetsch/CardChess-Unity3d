using App.Agent;

namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's HandModel onto the BoardAgent
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICardAgent CardAgent;
        public Coord Coord;

        public PlayCard()
        {
        }

        public PlayCard(ICardAgent cardAgent, Coord coord)
        {
            // TODO Assert.IsNotNull(card);
            CardAgent = cardAgent;
            Coord = coord;
        }

        public override string ToString()
        {
            return $"PlayCard {CardAgent} to {Coord}";
        }
    }
}
