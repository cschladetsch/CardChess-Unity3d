using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Action;
using App.Agent;
using App.Common;
using App.Model.Card;

namespace App.Model
{
    class PieceModel
        : CardModel,
        IPieceModel
    {
        public Coord Coord { get; }
        public IBoardModel Board { get; }

        public PieceModel(ICardModelTemplate modelTemplate, IOwner owner) : base(modelTemplate, owner)
        {
        }
    }
}
