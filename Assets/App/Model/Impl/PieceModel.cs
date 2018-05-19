using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common;
using Flow;

namespace App.Model
{
    public class PieceModel
        : ModelBase
            , IPieceModel
    {
        public ICardModel Card { get; }
        public EPieceType Type => Card.PieceType;
        public IBoardModel Board { get; }
        public Coord Coord { get; }


        public IGenerator Battle(ICardModel other)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Attacking()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Defending()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Attackers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Defenders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPieceModel> Neareby(int distance)
        {
            throw new NotImplementedException();
        }
    }
}
