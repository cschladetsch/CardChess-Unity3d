using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public delegate void CardDelegate(object sender, ICard subject, params ICard[] context);

    public interface ICard :
    	Common.ICard, IModel
    {
        //#region Events
        //event CardDelegate Born;
        //event CardDelegate Died;
        //event CardDelegate Reborn;
        //event CardDelegate Moved;
        //event CardDelegate AppliedToPiece;
        //event CardDelegate RemovedFromPiece;
        //event CardDelegate HealthChanged;
        //event CardDelegate AttackChanged;
        //event CardDelegate ItemAdded;
        //event CardDelegate ItemRemoved;
        //event CardDelegate Attacked;
        //event CardDelegate Defended;
        //#endregion

        #region Properties
        ICardTemplate Template { get; }
        IEnumerable<ICard> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        #endregion

        #region Methods
        void ChangeHealth(int amount, ICard cause);
        #endregion
    }
}
