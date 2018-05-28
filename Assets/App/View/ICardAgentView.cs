using App.Agent;
using App.View.Impl;

namespace App.View
{
    /// <summary>
    /// View of a card in the scene. This
    /// can be in a Hand, a Deck, Graveyard or
    /// transitioning into a Piece, Item or Spell
    /// on the board
    /// </summary>
    public interface ICardAgentView
        : IView<ICardAgent>
    {
    }
}
