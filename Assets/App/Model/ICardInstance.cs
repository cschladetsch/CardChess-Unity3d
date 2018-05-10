using System.Collections.Generic;
using System.ComponentModel;

namespace App.Model
{
    public delegate void CardInstanceDelegate(object sender, ICardInstance subject, params ICardInstance[] context);

    public interface ICardInstance : IHasId, IOwned
    {
        event CardInstanceDelegate Born;
        event CardInstanceDelegate Died;
        event CardInstanceDelegate Reborn;
        event CardInstanceDelegate Moved;
        event CardInstanceDelegate AppliedToPiece;
        event CardInstanceDelegate RemovedFromPiece;
        event CardInstanceDelegate HealthChanged;
        event CardInstanceDelegate AttackChanged;
        event CardInstanceDelegate ItemAdded;
        event CardInstanceDelegate ItemRemoved;
        event CardInstanceDelegate Attacked;
        event CardInstanceDelegate Defended;

        ICardTemplate Template { get; }

        int Attack { get; set; }
        int Health { get; }
        IList<ICardInstance> Items { get; }
        IList<EAbility> Abilities { get; }

        void ChangeHealth(int amount, ICardInstance cause);
    }
}
