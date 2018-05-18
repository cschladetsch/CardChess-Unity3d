using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    public interface ICardModel :
        IModel,
        Common.ICard
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

        ICardModelTemplate ModelTemplate { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }

        Response ChangeHealth(int amount, ICardModel cause);
    }

    public interface ICardModelMountable
        : ICardModel
    {
        ICardModel Mounted { get; }
        bool CanMount(ICardModel other);
        bool SetMounted(ICardModel rider);
    }
}
