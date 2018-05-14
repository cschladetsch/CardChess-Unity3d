using System.Collections.Generic;

namespace App.Model
{
    public delegate void CardInstanceDelegate(object sender, ICardInstance subject, params ICardInstance[] context);

    public interface ICardInstance : IModel, IOwned, IHasName
    {
        #region Events
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
        #endregion

        #region Properties
        ICardTemplate Template { get; }
        int Attack { get; }
        int Health { get; }
        IList<ICardInstance> Items { get; }
        IList<EAbility> Abilities { get; }
        #endregion

        #region Methods
        void ChangeHealth(int amount, ICardInstance cause);
        #endregion
    }
}
