using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App.Action
{
    public class RejectCards : ActionBase
    {
        public ICardModel[] Rejected { get; }

        public RejectCards(IPlayerModel player, IEnumerable<ICardModel> rejected)
            : base(player, EActionType.RejectCards)
        {
            Rejected = rejected.ToArray();
        }
    }
}
