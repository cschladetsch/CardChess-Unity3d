using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model.Card
{
    public class PieceModel
        : ModelBase
    {
        public ICardModel CardModel { get; }
    }

    public class King
        : PieceModel
    {
    }
}
