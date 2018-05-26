using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App.Common.Message
{
    public class RejectCards : RequestBase
    {
        public ICardModel[] Rejected { get; }

        public RejectCards(IPlayerModel player, IEnumerable<ICardModel> rejected)
            : base(player, EActionType.RejectCards)
        {
            Rejected = rejected.ToArray();
        }
    }
}
