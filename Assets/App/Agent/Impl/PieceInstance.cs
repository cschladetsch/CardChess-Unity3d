using System.Collections;
using System.Collections.Generic;
using App.Action;
using App.Model;
using Flow;

namespace App.Agent
{
    /// <summary>
    /// A piece on the board.
    /// </summary>
    public abstract class PieceInstance :
        CardInstance,
        IPieceInstance
    {
        #region Public Fields
        public IOwner Owner => Model.Owner;
        public int Health => Model.Health;
        public int Attack => Model.Attack;
        public ECardType Type => Model.Template.Type;
        public Coord Coord { get; private set; }
        #endregion

        #region Public Abstract Methods
        /// <summary>
        /// Where this card can move to
        /// </summary>
        public abstract IEnumerable<Coord> PotentialCoords();

        /// <summary>
        /// What attacks this card from the given coord
        /// </summary>
        /// <param name="coord">From this coord</param>
        public abstract IEnumerable<IPieceInstance> Attackers(Coord coord);

        /// <summary>
        /// What this card can attack
        /// </summary>
        /// <param name="coord"></param>
        public abstract IEnumerable<IPieceInstance> Defenders(Coord coord);

        /// <summary>
        /// Guards defending this piece from the given coord
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public abstract IEnumerable<IPieceInstance> Guards(Coord coord);
        #endregion

        #region Protected Methods
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
        #endregion

        #region Private Fields

        #endregion
    }
}
