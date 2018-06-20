using System.Linq;
using System.Collections.Generic;

namespace App.Common.Message
{
    using Model;

    public class RejectCards : RequestBase
    {
        public ICardModel[] Rejected { get; }

        public RejectCards(IPlayerModel player, IEnumerable<ICardModel> rejected = null)
            : base(player, EActionType.RejectCards)
        {
            if (rejected != null)
                Rejected = rejected.ToArray();
        }
    }
}
