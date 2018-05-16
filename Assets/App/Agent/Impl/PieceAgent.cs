using System.Collections;
using System.Collections.Generic;
using App.Action;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;

    /// <summary>
    /// A piece on the board.
    /// Possibly wearing a blue beret.
    /// </summary>
    public abstract class PieceAgent :
        CardInstance,
        IPieceInstance
    {
        #region Public Fields
        public IOwner Owner => Model.Owner;
        public int Attack => Model.Attack;
        public string Description => Model.Template.FlavourText;
        public ECardType Type => Model.Template.Type;
        public Coord Coord { get; private set; }
        public IList<IEffect> Effects { get; } = new List<IEffect>();
        #endregion

        #region Public Methods

        public bool SameOwner(IOwner other)
        {
            return Owner == other;
        }

        public IFuture<Action.MovePiece> Move()
        {
            throw new System.NotImplementedException();
        }

        #region Abstract Methods
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
